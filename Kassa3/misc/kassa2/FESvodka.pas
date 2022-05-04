//24.02 Fixed: Ошибка калькуляции итоговых сумм
//24.02 Fixed: Ошибка выборки
unit FESvodka;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ComCtrls, ExtCtrls, Grids, DBGridEh, RXCtrls, ActnList, StdCtrls,
  sysrelate, DB, RxMemDS, adsrelate, adsdata, adsfunc, adstable, ImgList,
  dlgrelate, PrnDbgeh;

{$WARNINGS OFF}
type
  TESvodka = class(TForm)
    Panel1: TPanel;
    Panel2: TPanel;
    sb: TStatusBar;
    dge: TDBGridEh;
    rsbUsl: TRxSpeedButton;
    al: TActionList;
    aUslovia: TAction;
    rsbPrintSvodka: TRxSpeedButton;
    aPrintSvodka: TAction;
    GroupBox1: TGroupBox;
    Label1: TLabel;
    lUsl: TLabel;
    lInterval: TLabel;
    Panel3: TPanel;
    rsbFit: TRxSpeedButton;
    md: TRxMemoryData;
    dsmd: TDataSource;
    il1: TImageList;
    pdbg: TPrintDBGridEh;
    rsbExpCsv: TRxSpeedButton;
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure rsbFitClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure dgeDrawColumnCell(Sender: TObject; const Rect: TRect;
      DataCol: Integer; Column: TColumnEh; State: TGridDrawState);
    procedure aUsloviaExecute(Sender: TObject);
    procedure aPrintSvodkaExecute(Sender: TObject);
    procedure dgeSortMarkingChanged(Sender: TObject);
    constructor Create(aOwner: TComponent; aSvData: string);
    procedure rsbExpCsvClick(Sender: TObject);
  private
    procedure Create_Column(nm, na: string; i1, i2: currency);
    function Make_Where: string;
    procedure create_simple_col(nm, na: string; wh: integer);
    { Private declarations }
  public
    { Public declarations }
    SvData: string;
    procedure refresh_controls;
    procedure refresh_svodka;
    function gv(aName, aDefault: string): string;
    destructor Destroy; override;
  end;
{$WARNINGS ON}

var
  ESvodka: TESvodka;

implementation

uses Unit1, Unit2, FESvParam;

{$R *.dfm}

function TESvodka.gv(aName,aDefault: string):string;
var
    sl:tstringlist;
begin
    sl:=tstringlist.create;
    sl.text:=SvData;
    result:=getvalue(sl, aName, aDefault);
    sl.free;
end;

procedure TESvodka.refresh_controls;
const
    sco:array[0..1] of string=('Контрагенты','Счета');
var
    dbg,ded: tdatetime;
    sc: integer;
begin
    try
        dbg:=strtodate(gv('DateBegin',datetostr(now)));
    except dbg:=trunc(now);
    end;
    try
        ded:=strtodate(gv('DateEnd',datetostr(now)));
    except ded:=trunc(now);
    end;
    lInterval.caption:=datetostr(dbg);
    if dbg<ded then lInterval.caption:=linterval.caption+' - '+datetostr(ded);
    try
        sc:=strtoint(gv('Option','0'));
    except sc:=0;
    end;
    lUsl.Caption:=sco[sc];
    caption:='Сводка: '+gv('title','');
    dge.autofitcolwidths:=rsbFit.down;
    if not rsbfit.Down then AutoColWidth(dge,10,10)
end;



procedure TESvodka.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    action:=cafree;
end;

destructor TESvodka.Destroy;
begin
  inherited Destroy;
  Form1.Do_windows_menu;
end;

constructor TESvodka.Create(aOwner: TComponent; aSvData: string);
begin
    inherited Create(aOwner);
    SvData:=aSvData;
end;

procedure TESvodka.rsbFitClick(Sender: TObject);
begin
    refresh_controls;
end;

//==============================================================================
//===== Создание сводки ========================================================
//==============================================================================
function ret_acc(atfld,atacc,alst: string; aopt: integer):string;
const
    obtw: array[0..1] of string=('=3',' BETWEEN 0 AND 2');
var
    ps:pparsedstr;
    f:integer;
begin
    result:='';
    ps:=parsestring(alst,';');
    if ps.count>0 then begin
        for f:=0 to ps.count-1 do begin
            if result<>'' then result:=result+' or ';
            result:=result+'('+atacc+'='+ps.sa[f]+')';
        end;
        if ps.count>1 then result:='('+result+')';
        result:='(('+atfld+obtw[aopt]+') AND '+result+')';
    end;
end;

function ret_opid(alst:string):string;
var
    ps:pparsedstr;
    f:integer;
begin
    result:='';
    ps:=parsestring(alst,';');
    if ps.count>0 then begin
        for f:=0 to ps.count-1 do begin
            if result<>'' then result:=result+' or ';
            result:=result+'(Op_ID='+ps.sa[f]+')';
        end;
        if ps.count>1 then result:='('+result+')';
    end;
end;

function ret_date(seb,see: string): string;
var
    deb,dee: tdatetime;
begin
    try deb:=strtodate(seb);
    except deb:=trunc(now);
    end;
    try dee:=strtodate(see);
    except dee:=trunc(now);
    end;
    result:='((K_Date>'+quotedstr(datetostr(deb-1))+') and (K_Date<'+
            quotedstr(datetostr(dee+1))+'))';
end;

function tesvodka.Make_Where:string;
const
    flopt: array[0..1] of string=('ClientList','AccountList');
var
    opt: integer;
begin
    try opt:=strtoint(gv('Option','0'));
    except opt:=0;
    end;
    result:='('+ret_acc('SrcType','SrcAcc',gv(flopt[opt],''),opt)+' OR '+
        ret_acc('DstType','DstAcc',gv(flopt[opt],''),opt)+') and '+
        ret_date(gv('DateBegin',datetostr(now)),gv('DateEnd',datetostr(now)))+
        ' AND '+ret_opid(gv('Oplist',''));
end;

procedure tesvodka.refresh_svodka;
const
    cldst: array[0..2] of char=('n','b','u');
type
    curec=array[0..2] of currency;
var
    wh: string;
    q, qcl, qac: tadsquery;
    acs: array of integer;
    acp,acr: array of curec;
    opt,f,g,h: integer;
    sac,dac: string;
    sho:tform;
    gps:pparsedstr;
        
procedure Add_acs(aid: integer);
var
    lf:integer;
begin
    if length(acs)>0 then
        for lf:=0 to length(acs)-1 do
            if aid=acs[lf] then exit;
    setlength(acs,length(acs)+1);
    acs[length(acs)-1]:=aid;
end;

function i2n(aid: integer):integer;
var
    lf: integer;
begin
    result:=-1;
    for lf:=0 to length(acs) do
        if acs[lf]=aid then begin
            result:=lf;
            exit;
        end;
end;

begin
    dge.visible:=false;
    dge.Columns.Clear;
    wh:=make_where;
    setlength(acs,0);
    try opt:=strtoint(gv('Option','0'));
    except opt:=0;
    end;

    if ansisametext(gv('RestrictAcc',yn[false]),yn[true]) then opt:=1;

    sho:=showprogressmsg('Подготовка сводки',0,100);
    application.processmessages;

    case opt of
        0:  begin
                q:=adsq(datadir,'select id, srcacc, dstacc, srctype, dsttype from "'+
                        kas_name+'" where '+wh);
                if not emp(q) then begin
                    setprogress(sho,0,0,q.recordcount*2);
                    application.ProcessMessages;
                    h:=0;
                    q.first;
                    while not q.eof do begin
                        if q.fieldbyname('SrcType').asinteger in [0..2] then
                            Add_acs(q.fieldbyname('SrcAcc').asinteger);
                        if q.fieldbyname('DstType').asinteger in [0..2] then
                            Add_acs(q.fieldbyname('DstAcc').asinteger);
                        q.next;
                        inc(h);
                        if h mod 50 = 0 then begin
                            setprogress(sho,h);
                            application.processmessages;
                        end;
                    end;
                end;
                fsq(q);
            end;
        1:  begin
                gps:=parsestring(gv('AccountList',''),';');
                if gps.count>0 then
                    for f:=0 to gps.count-1 do
                        try add_acs(strtoint(gps.sa[f])); except end;
                freeparsedstr(gps);
            end;
    end;
    if length(acs)>0 then begin
        setlength(acp,length(acs));
        setlength(acr,length(acs));
        for f:=0 to length(acs)-1 do
            for g:=0 to 2 do begin
                acp[f][g]:=0;
                acr[f][g]:=0;
            end;
        md.Close;
        with md.FieldDefs do begin
            clear;
            add('ID',ftInteger);
            add('Date',ftDate);
            add('Opname',ftString,240);
            add('Optype',ftBoolean);
            add('ClName',ftstring,240);
            add('Prim',ftstring,240);
            add('bmk_id',ftInteger);
            for f:=0 to length(acs)-1 do begin
                add('c'+inttostr(acs[f])+'np',ftcurrency);
                add('c'+inttostr(acs[f])+'nr',ftcurrency);
                add('c'+inttostr(acs[f])+'bp',ftcurrency);
                add('c'+inttostr(acs[f])+'br',ftcurrency);
                add('c'+inttostr(acs[f])+'up',ftcurrency);
                add('c'+inttostr(acs[f])+'ur',ftcurrency);
            end;
        end;
        md.Open;
        md.DisableControls;

        q:=adsq(datadir,'select kas.*, ops.optype oot, ops.name oon from "'+kas_name+
                '" kas, "'+ops_name+'" ops where ('+wh+') and (ops.id=kas.Op_ID)');
//oot = тип операции
//oon = наименование операции
        qcl:=adsq(datadir,'select id, c_name from "'+cli_name+'"');
        qac:=adsq(datadir,'select id, name from "'+acc_name+'"');
        if (q<>nil) and (qcl<>nil) and (qac<>nil) then begin
            if (q.active) and (q.recordcount>0) then begin
                setprogress(sho,q.RecordCount,0,q.RecordCount*2);
                application.processmessages;
                h:=q.recordcount;
                q.first;
                while not q.eof do begin
                if ((q.fieldbyname('srctype').asinteger in [0..2]) and
                    (i2n(q.fieldbyname('srcacc').AsInteger)>-1)) or
                   ((q.fieldbyname('dsttype').asinteger in [0..2]) and
                    (i2n(q.fieldbyname('dstacc').AsInteger)>-1)) then begin
                        md.append;
                        for f:=0 to length(acs)-1 do begin
                            md.fieldbyname('c'+inttostr(acs[f])+'nr').ascurrency:=0;
                            md.fieldbyname('c'+inttostr(acs[f])+'np').ascurrency:=0;
                            md.fieldbyname('c'+inttostr(acs[f])+'br').ascurrency:=0;
                            md.fieldbyname('c'+inttostr(acs[f])+'bp').ascurrency:=0;
                            md.fieldbyname('c'+inttostr(acs[f])+'ur').ascurrency:=0;
                            md.fieldbyname('c'+inttostr(acs[f])+'up').ascurrency:=0;
                        end;
                        md.fieldbyname('id').AsInteger:=q.fieldbyname('id').AsInteger;
                        md.fieldbyname('date').asdatetime:=q.fieldbyname('k_date').AsDateTime;
                        md.fieldbyname('opname').AsString:=q.fieldbyname('oon').AsString;
                        md.fieldbyname('optype').AsBoolean:=q.fieldbyname('oot').AsBoolean;
                        md.fieldbyname('prim').AsString:=q.fieldbyname('prim').AsString;

                        if (q.fieldbyname('srctype').AsInteger in [0..2]) and
                           (q.fieldbyname('dsttype').AsInteger in [0..2]) then begin
    //Операция между счетами
                            if qac.locate('ID',q.fieldbyname('srcacc').AsInteger,[]) then
                                sac:=qac.fieldbyname('name').AsString;
                            if qac.locate('ID',q.fieldbyname('dstacc').AsInteger,[]) then
                                dac:=qac.fieldbyname('name').AsString;
                            md.fieldbyname('ClName').AsString:=sac+' >> '+dac;

                            if ((q.fieldbyname('srctype').asinteger in [0..2]) and
                                (i2n(q.fieldbyname('srcacc').AsInteger)>-1)) then begin
                                md.fieldbyname('c'+inttostr(q.fieldbyname('srcacc').asinteger)+
                                                cldst[q.fieldbyname('srctype').asinteger]+'r'
                                               ).AsCurrency:=q.fieldbyname('K_Sum').AsCurrency;
                                acr[i2n(q.fieldbyname('srcacc').asinteger)][q.fieldbyname('srctype').asinteger]:=
                                    acr[i2n(q.fieldbyname('srcacc').asinteger)][q.fieldbyname('srctype').asinteger]+
                                    q.fieldbyname('K_Sum').AsCurrency;
                            end;

                            if ((q.fieldbyname('dsttype').asinteger in [0..2]) and
                                (i2n(q.fieldbyname('dstacc').AsInteger)>-1)) then begin
                                md.fieldbyname('c'+inttostr(q.fieldbyname('dstacc').asinteger)+
                                                cldst[q.fieldbyname('dsttype').asinteger]+'p'
                                               ).AsCurrency:=q.fieldbyname('K_Sum').AsCurrency;
                                acp[i2n(q.fieldbyname('dstacc').asinteger)][q.fieldbyname('dsttype').asinteger]:=
                                    acp[i2n(q.fieldbyname('dstacc').asinteger)][q.fieldbyname('dsttype').asinteger]+
                                    q.fieldbyname('K_Sum').AsCurrency;
                            end;

                        end else begin
                            if q.fieldbyname('oot').AsBoolean then begin
                        //Приход (??)
                                if (q.fieldbyname('srctype').AsInteger=3) and
                                   (qcl.Locate('ID',inttostr(q.fieldbyname('srcacc').AsInteger),[]))
                                    then md.fieldbyname('ClName').AsString:=qcl.fieldbyname('c_name').AsString
                                    else md.fieldbyname('clname').AsString:='???';
                                md.fieldbyname('c'+inttostr(q.fieldbyname('dstacc').asinteger)+
                                                cldst[q.fieldbyname('dsttype').asinteger]+'p'
                                               ).AsCurrency:=q.fieldbyname('K_Sum').AsCurrency;
                                acp[i2n(q.fieldbyname('dstacc').asinteger)][q.fieldbyname('dsttype').asinteger]:=
                                    acp[i2n(q.fieldbyname('dstacc').asinteger)][q.fieldbyname('dsttype').asinteger]+
                                    q.fieldbyname('K_Sum').AsCurrency;
                            end else begin
                        //Расход
                                if (q.fieldbyname('dsttype').AsInteger=3) and
                                   (qcl.Locate('ID',inttostr(q.fieldbyname('dstacc').AsInteger),[]))
                                   then md.fieldbyname('ClName').AsString:=qcl.fieldbyname('c_name').AsString
                                   else md.fieldbyname('clname').AsString:='???';
                                md.fieldbyname('c'+inttostr(q.fieldbyname('srcacc').asinteger)+
                                                cldst[q.fieldbyname('srctype').asinteger]+'r'
                                               ).AsCurrency:=q.fieldbyname('K_Sum').AsCurrency;
                                acr[i2n(q.fieldbyname('srcacc').asinteger)][q.fieldbyname('srctype').asinteger]:=
                                    acr[i2n(q.fieldbyname('srcacc').asinteger)][q.fieldbyname('srctype').asinteger]+
                                    q.fieldbyname('K_Sum').AsCurrency;
                            end;
                        end;

                        md.post;
                    end;
                    q.next;
                    inc(h);
                    if h mod 50 = 0 then begin
                        setprogress(sho,h);
                        application.processmessages;
                    end;
                end;
            end;
        end;
        if q<>nil then begin
            q.Close;
            q.free;
        end;
        if md.RecordCount>0 then begin
//Создание колонок
            Create_simple_col('Date','Операция|Дата',64);
            Create_simple_col('Opname','Операция|Наименование',150);
            Create_simple_col('Optype','Операция|Тип',20);

            Create_simple_col('Clname','Клиент/счёт|Имя',120);
            Create_simple_col('Prim','Клиент/счёт|Примечание',150);

            for f:=0 to length(acs)-1 do begin
                if qac.Locate('ID',acs[f],[]) then
                    sac:=qac.fieldbyname('name').asstring else sac:='?';
                Create_column('c'+inttostr(acs[f])+'np',sac+'|Наличные|Приход',acp[f][0],0);
                Create_column('c'+inttostr(acs[f])+'nr',sac+'|Наличные|Расход',acr[f][0],acp[f][0]-acr[f][0]);
                Create_column('c'+inttostr(acs[f])+'bp',sac+'|Безналичные|Приход',acp[f][1],0);
                Create_column('c'+inttostr(acs[f])+'br',sac+'|Безналичные|Расход',acr[f][1],acp[f][1]-acr[f][1]);
                Create_column('c'+inttostr(acs[f])+'up',sac+'|Валюта|Приход',acp[f][2],0);
                Create_column('c'+inttostr(acs[f])+'ur',sac+'|Валюта|Расход',acr[f][2],acp[f][2]-acr[f][2]);
            end;
        end;
        if qac<>nil then begin
            qac.close;
            qac.free;
        end;
        if qcl<>nil then begin
            qcl.close;
            qcl.free;
        end;
        dge.visible:=md.recordcount>0;
        md.enablecontrols;
    end;
    sho.free;
end;

procedure tesvodka.create_simple_col(nm,na:string; wh: integer);
begin
    with dge.columns.add do begin
        FieldName:=nm;
        title.Caption:=na;
        width:=wh;
        with Footers do begin
            clear;
            with add do begin
                valuetype:=fvtstatictext;
                Value:='';
                Color:=clltgray;
            end;
            with add do begin
                valuetype:=fvtstatictext;
                Value:='';
                Color:=clltgray;
            end;
        end;
    end;
end;

procedure tESvodka.Create_Column(nm,na:string;i1,i2:currency);
begin
    with dge.Columns.Add do begin
        FieldName:=nm;
        title.Caption:=na;
        DisplayFormat:='# ### ### ##0.00';
        width:=60;
        with Footers do begin
            clear;
            with add do begin
                valuetype:=fvtstatictext;
                Value:=formatfloat('# ### ### ##0.00',i1);
                Color:=clltgray;
                font.Color:=clblack;
            end;

            with add do begin
                valuetype:=fvtstatictext;
                if nm[length(nm)]='r' then begin
                    Value:=formatfloat('# ### ### ##0.00',i2);
                    if i2>0 then Color:=$c0ffc0 else
                        if i2=0 then Color:=clltgray else
                            if i2<0 then color:=$ffc0c0;
                end else begin
                    Color:=clltgray;
                    value:='';
                end;
                font.Color:=clblack;
            end;
        end;
    end;
end;

procedure TESvodka.FormShow(Sender: TObject);
begin
    refresh_svodka;
    if not rsbfit.Down then AutoColWidth(dge,10,10);
    refresh_controls;
    Form1.Do_windows_menu;
end;

procedure TESvodka.dgeDrawColumnCell(Sender: TObject; const Rect: TRect;
  DataCol: Integer; Column: TColumnEh; State: TGridDrawState);
const
    ft:array[false..true] of integer=(0,1);
begin
    if ansisametext(column.Field.FieldName,'optype') then begin
        dge.Canvas.FillRect(rect);
        il1.Draw(dge.canvas,rect.left+1,rect.top+1,ft[column.field.asboolean]);
    end;
end;

procedure TESvodka.aUsloviaExecute(Sender: TObject);
begin
    esvparam.SvData:=svdata;
    if esvparam.ShowModal=mrok then begin
        svdata:=esvparam.SvData;
        refresh_svodka;
        refresh_controls;
        Form1.Do_windows_menu;
    end;
end;

procedure TESvodka.aPrintSvodkaExecute(Sender: TObject);
begin
    pdbg.Title.Clear;
    pdbg.title.add(caption);
    pdbg.Preview;
end;

procedure TESvodka.dgeSortMarkingChanged(Sender: TObject);
var
    f:integer;
    sf: string;
    desc: boolean;
begin
    sb.SimpleText:='Сортировка...';
    sf:='';
    desc:=false;
    for f:=0 to dge.SortMarkedColumns.Count-1 do begin
        desc:=(dge.SortMarkedColumns[f].Title.SortMarker = smUpEh);
        if sf<>'' then sf:=sf+';';
        sf:=sf+dge.SortMarkedColumns[f].FieldName;
    end;
    if sf<>'' then begin
        md.disablecontrols;
        md.SortOnFields(sf,true,desc);
        md.enablecontrols;
    end;
    Refresh_controls;
    sb.simpletext:='';
end;

procedure TESvodka.rsbExpCsvClick(Sender: TObject);
begin
  dm.SaveDbgToCsv(dge);
end;

end.
