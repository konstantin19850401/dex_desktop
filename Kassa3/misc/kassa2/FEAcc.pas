unit FEAcc;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ComCtrls, ExtCtrls, RXCtrls, Menus, Grids, DBGridEh, PrnDbgeh, Db,
  RxMemDS, StdCtrls, Mask, ToolEdit, inifiles, adsdata, adsfunc, adstable,
  dlgrelate, adsrelate, sysrelate, ActnList;

const
    yn:array[false..true] of string=('false','true');
    mtxt: array[1..12] of string=('январь', 'февраль', 'март', 'апрель', 'май',
        'июнь', 'июль', 'август', 'сентябрь', 'октябрь', 'ноябрь', 'декабрь');
    dnd: array[1..7] of string=('воскресенье', 'понедельник', 'вторник',
        'среда', 'четверг', 'пятница', 'суббота');
    dvg:array[false..true,1..12] of integer=
        ((31,28,31,30,31,30,31,31,30,31,30,31),
         (31,29,31,30,31,30,31,31,30,31,30,31));

type
    tdy=record
        dweek: string;
        ix: integer;
    end;
    tmth=record
        count:integer;
        d:array[1..31] of tdy;
    end;
    tyr=record
        year: word;
        mth:array[1..12] of tmth;
    end;

  TComRefProc=procedure (jid:integer; aaction: integer) of object;

{$Warnings off}
  TEAcc = class(TForm)
    Panel1: TPanel;
    Panel2: TPanel;
    sb: TStatusBar;
    rsbUsl: TRxSpeedButton;
    rsbPrint: TRxSpeedButton;
    Panel3: TPanel;
    rsbFit: TRxSpeedButton;
    pmUsl: TPopupMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    N3: TMenuItem;
    N4: TMenuItem;
    N5: TMenuItem;
    N6: TMenuItem;
    N7: TMenuItem;
    dge1: TDBGridEh;
    md: TRxMemoryData;
    mdDate: TDateTimeField;
    mdName: TStringField;
    mdOptype: TBooleanField;
    mdPrim: TStringField;
    mdNp: TCurrencyField;
    mdNr: TCurrencyField;
    mdBp: TCurrencyField;
    mdBr: TCurrencyField;
    mdUp: TCurrencyField;
    mdUr: TCurrencyField;
    mdJournal_Id: TIntegerField;
    mdOptypeS: TStringField;
    mdOpname: TStringField;
    ds1: TDataSource;
    pdb: TPrintDBGridEh;
    dtpm: TPopupMenu;
    Label1: TLabel;
    deStart: TDateEdit;
    Label2: TLabel;
    deEnd: TDateEdit;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    rsbRefreshView: TRxSpeedButton;
    mdbmk_id: TIntegerField;
    al2: TActionList;
    aPrintAcc: TAction;
    N8: TMenuItem;
    Label7: TLabel;
    aOrder: TAction;
    pmSub: TPopupMenu;
    F51: TMenuItem;
    RxSpeedButton1: TRxSpeedButton;
    aBookmark: TAction;
    mdusrcr: TStringField;
    mdusrch: TStringField;
    N9: TMenuItem;
    Label8: TLabel;
    N10: TMenuItem;
    aIntOrder: TAction;
    mdr_cr: TStringField;
    rsbExpCsv: TRxSpeedButton;
    procedure N5Click(Sender: TObject);
    procedure N6Click(Sender: TObject);
    procedure N7Click(Sender: TObject);
    procedure N3Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure FormShow(Sender: TObject);
    procedure rsbFitClick(Sender: TObject);
    procedure rsbRefreshViewClick(Sender: TObject);
    procedure dge1KeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure dge1SortMarkingChanged(Sender: TObject);
    procedure dge1DblClick(Sender: TObject);
    procedure deStartAcceptDate(Sender: TObject; var ADate: TDateTime;
      var Action: Boolean);
    procedure aPrintAccExecute(Sender: TObject);
    procedure N8Click(Sender: TObject);
    procedure dge1GetCellParams(Sender: TObject; Column: TColumnEh;
      AFont: TFont; var Background: TColor; State: TGridDrawState);
    procedure aOrderExecute(Sender: TObject);
    procedure N9Click(Sender: TObject);
    procedure aIntOrderExecute(Sender: TObject);
    procedure rsbExpCsvClick(Sender: TObject);
  private
    FComRefProc:TComRefProc;
    procedure LoadSettings;
    procedure Refresh_view;
    procedure refresh_controls;
    procedure SaveSettings;
    procedure Add_md_rec(aqac, aqcl: tadsquery; fkaid: integer;
      fk_date: tdatetime; fprim, fopname: string; fbmk_id, fsrcacc,
      fdstacc, fsrctype, fdsttype: integer; fsn, fsb, fsu, fdn, fdb,
      fdu: currency; fusrcr, fusrch, r_cr: string); // SyncMod 29.04.2008
    procedure Do_Delete_Record(jid, abmk: integer; ats: string); // SyncMod 29.04.2008
    procedure Do_Modify_Record(jid: integer);
    function colbyname(n: string): tcolumneh;
    procedure refreshsum;
    procedure Create_days;
    procedure dyclick(Sender: TObject);
    function Get_Pacient(aAcc, aType: integer): string;
    function Get_Passport(aAcc, aType: integer): string;
    procedure Fill_Days;
    procedure AddD(td: tdatetime);
    function Check_user_rights(jid: integer): string;
    procedure LogDeletedRecord(aID: integer);
    procedure DeleteOrders(jid, doctype: integer);
    { Private declarations }
  public
    { Public declarations }
    acc_id: integer;
    acc_title: string;
    _bmk: integer;
    view_option: integer;
    _order: string;
    constructor Create(AOWner: TComponent; aacc_id: integer; aacc_name: string; aComRefProc: TComRefProc);
    procedure Refresh_data;
    procedure Do_Refresh_Account(jid: integer; aaction: integer);
    procedure MakeRashodOrder(jid: integer);
    procedure MakePrihodOrder(jid: integer);
    destructor Destroy; override;
  end;
{$Warnings on}

var
  EAcc: TEAcc;
  yr:array of tyr;

implementation

uses Unit2, Unit1, FEAct, FECln, FEAgentDic;

{$R *.DFM}

procedure TEAcc.dyclick(Sender: TObject);
begin
    n3.checked:=true;
    view_option:=3;
    destart.date:=(sender as tmenuitem).tag;
    refresh_view;
end;

procedure TEAcc.AddD(td:tdatetime);
var
    yy,mm,dd: word;
    noy,hy,hm,hd: integer;
begin
    decodedate(td,yy,mm,dd);
    noy:=-1;
    if length(yr)>0 then
        for hy:=0 to length(yr)-1 do
            if yr[hy].year=yy then noy:=hy;
    if noy=-1 then begin
        noy:=length(yr);
        setlength(yr,noy+1);
        yr[noy].year:=yy;
        for hm:=1 to 12 do begin
            yr[noy].mth[hm].count:=dvg[isleapyear(yy),hm];
            for hd:=1 to yr[noy].mth[hm].count do begin
                yr[noy].mth[hm].d[hd].dweek:='';
                yr[noy].mth[hm].d[hd].ix:=-1;
            end;
        end;
    end;
    yr[noy].mth[mm].d[dd].dweek:=dnd[dayofweek(td)];
    yr[noy].mth[mm].d[dd].ix:=trunc(td);
end;

procedure TEAcc.Create_days;
var
    q:tadsquery;
begin
    setlength(yr,0);
    q:=adsq(datadir,'select distinct k_date from "'+kas_name+'" where '+
            '(((srctype between 0 and 2) and (srcacc='+inttostr(acc_id)+')) or '+
            ' ((dsttype between 0 and 2) and (dstacc='+inttostr(acc_id)+'))) '+
            'order by k_date');
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            q.first;
            while not q.Eof do begin
                addd(q.fieldbyname('k_date').AsDateTime);
                q.next;
            end;
            Fill_days;
        end;
        q.Close;
        q.free;
    end;
end;

procedure TEAcc.Fill_Days;
var
    qy,qm,qmc,qd: integer;
    my,mm,mkd:tmenuitem;
begin
    n3.Clear;
    if length(yr)>0 then begin
        for qy:=0 to length(yr)-1 do begin
            my:=newitem(inttostr(yr[qy].year)+' год',0,false,true,nil,0,'yr'+inttostr(yr[qy].year));
            n3.Add(my);
            for qm:=1 to 12 do begin
                mm:=newitem(mtxt[qm],0,false,true,nil,0,'mth'+inttostr(yr[qy].year)+lz(qm,2));
                qmc:=0;
                for qd:=1 to yr[qy].mth[qm].count do
                    if yr[qy].mth[qm].d[qd].ix>-1 then begin
                        inc(qmc);
                        mkd:=newitem(inttostr(qd)+', '+yr[qy].mth[qm].d[qd].dweek,0,false,true,dyclick,0,'day'+inttostr(yr[qy].mth[qm].d[qd].ix));
                        mkd.Tag:=yr[qy].mth[qm].d[qd].ix;
                        mm.Add(mkd);
                    end;
                if qmc>0 then my.Add(mm) else mm.free;
            end;
        end;
    end;
    n3.Enabled:=length(yr)>0;
end;

procedure TEAcc.refresh_controls;
begin
    caption:='Счёт: '+acc_title;
    if not(view_option in [1..3]) then view_option:=3;
    n1.checked:=view_option=1;
    n2.checked:=view_option=2;
    n3.checked:=view_option=3;
    label4.visible:=n5.checked;
    label5.visible:=n6.checked;
    label6.visible:=n7.checked;
    label7.visible:=n8.Checked;
    label8.visible:=n9.Checked;
    label1.visible:=view_option in [1,3];
    destart.visible:=view_option in [1,3];
    label2.visible:=view_option=1;
    deend.visible:=view_option=1;
    rsbrefreshview.visible:=view_option=1;
    dge1.autofitcolwidths:=rsbFit.down;
    if not rsbfit.down then AutoColWidth(dge1,5,5);
    create_days;
end;

procedure TEAcc.Add_md_rec(aqac,aqcl: tadsquery;
          fkaid:integer; fk_date: tdatetime; fprim, fopname: string;
          fbmk_id, fsrcacc, fdstacc, fsrctype, fdsttype: integer;
          fsn,fsb,fsu,fdn,fdb,fdu: currency; fusrcr, fusrch, r_cr: string); // SyncMod 29.04.2008

const
    rpr:array[false..true] of string=('Р','П');
var
    opnm,optp:string;

function gpacient(aAcc,aTp: integer):string;
begin
    result:='?';
    case aTp of
        0,1,2: if (aqac<>nil) and (aqac.Locate('id',aacc,[])) then result:=aqac.fieldbyname('name').asstring;
        3: if (aqcl<>nil) and (aqcl.Locate('id',aacc,[])) then result:=aqcl.fieldbyname('c_name').asstring;
    end;
end;

begin
    md.append;
    md.fieldbyname('id').asinteger:=fkaid;
    md.fieldbyname('date').asdatetime:=fk_date;
    md.fieldbyname('prim').asstring:=fprim;
    md.fieldbyname('opname').asstring:=fopname;
    md.fieldbyname('bmk_id').asinteger:=fbmk_id;
    md.fieldbyname('Np').ascurrency:=0;
    md.fieldbyname('Nr').ascurrency:=0;
    md.fieldbyname('Bp').ascurrency:=0;
    md.fieldbyname('Br').ascurrency:=0;
    md.fieldbyname('Up').ascurrency:=0;
    md.fieldbyname('Ur').ascurrency:=0;
//Расход
    if (fsrcacc=acc_id) and (btw(fsrctype,0,2)) then begin
        md.fieldbyname('Optype').asboolean:=false;
        optp:=rpr[false];
        case fsrctype of
            0: md.fieldbyname('Nr').ascurrency:=fsn;
            1: md.fieldbyname('Br').ascurrency:=fsb;
            2: md.fieldbyname('Ur').ascurrency:=fsu;
        end;
        opnm:=gpacient(fdstacc,fdsttype);
    end;
//Приход
    if (fdstacc=acc_id) and (btw(fdsttype,0,2)) then begin
        md.fieldbyname('Optype').asboolean:=true;
        optp:=rpr[true];
        case fdsttype of
            0: md.fieldbyname('Np').ascurrency:=fdn;
            1: md.fieldbyname('Bp').ascurrency:=fdb;
            2: md.fieldbyname('Up').ascurrency:=fdu;
        end;
        opnm:=gpacient(fsrcacc,fsrctype);
    end;
    if (fsrcacc=fdstacc) and (fsrctype in [0..2]) and (fdsttype in [0..2]) then begin
        optp:='--';
        opnm:='<Внутри счёта>';
    end;
    md.fieldbyname('OptypeS').asstring:=optp;
    md.fieldbyname('Name').asstring:=opnm;
    md.fieldbyname('usrcr').AsString:=fusrcr;
    md.fieldbyname('usrch').AsString:=fusrch;
// SyncMod 29.04.2008
    md.FieldByName('r_cr').asstring := r_cr;
// SyncMod /    
    md.post;
end;

procedure TEAcc.Refresh_data;
var
    intop,prop,raop,showi,vnb,rab,prb: boolean;
    qusr, qac, qcl, qka:tadsquery;
    sho:tform;
    das,dae:tdatetime;
    zzz, urcr, urch, dsq: string;
    cucr: tcolumneh;
begin
    sho:=showwaitmsg('Запрос в БД.');
    dge1.visible:=false;
    try
        das:=strtodate(destart.text);
    except das:=now;
    end;
    try
        dae:=strtodate(deend.text);
    except dae:=das;
    end;
    case view_option of
        1: dsq:='((k_date>'+quotedstr(datetostr(das-1))+') and (k_date<'+
                quotedstr(datetostr(dae+1))+')) and ';
        3: dsq:='((k_date>'+quotedstr(datetostr(das-1))+') and (k_date<'+
                quotedstr(datetostr(das+1))+')) and ';
        else dsq:='';
    end;
    qac:=adsq(datadir,'select * from "'+acc_name+'"');
    qcl:=adsq(datadir,'select * from "'+cli_name+'"');
    qka:=tadsquery.create(application);
    qka.AdsRegisterProgressCallBack(DoAdsCallback);
    zzz :=
          'select '+
          'ka.id kaid, op_id, prim, k_date, srcacc, dstacc, srctype, dsttype, '+
          'sn, sb, su, sc, dn, db, du, dc, bmk_id, op.name opname, optype, '+
          'user_cr, user_ch, ka.r_cr r_cr from "'+kas_name+'" ka, "'+ops_name+'" op '+
          'where (op.id=ka.op_id) and '+ // SyncMod 29.04.2008
          dsq+
          '(((srctype between 0 and 2) and (srcacc='+inttostr(acc_id)+')) or '+
          ' ((dsttype between 0 and 2) and (dstacc='+inttostr(acc_id)+'))) '+
          'order by k_date';

    adsqq(qka,datadir, zzz);
    qusr:=adsq(datadir,format('select * from "%s"',[usr_name]));
    md.Close;
    md.Open;
    md.DisableControls;
    tcurrencyfield(md.fieldbyname('Np')).DisplayFormat:='# ### ### ##0.00';
    tcurrencyfield(md.fieldbyname('Nr')).DisplayFormat:='# ### ### ##0.00';
    tcurrencyfield(md.fieldbyname('Bp')).DisplayFormat:='# ### ### ##0.00';
    tcurrencyfield(md.fieldbyname('Br')).DisplayFormat:='# ### ### ##0.00';
    tcurrencyfield(md.fieldbyname('Up')).DisplayFormat:='# ### ### ##0.00';
    tcurrencyfield(md.fieldbyname('Ur')).DisplayFormat:='# ### ### ##0.00';
    rab:=n6.checked;
    prb:=n5.checked;
    vnb:=n8.checked;
    if not emp(qka) then
    begin

      qka.first;
      while not qka.eof do begin
  //Определение, нужно ли выводить операцию данного типа в сетку.
          intop:=((qka.fieldbyname('srcacc').asinteger=acc_id) and
                  (qka.fieldbyname('srcacc').asinteger=
                   qka.fieldbyname('dstacc').asinteger) and
                  (btw(qka.fieldbyname('SrcType').asinteger,0,2)) and
                  (btw(qka.fieldbyname('DstType').asinteger,0,2)));

          raop:=((qka.fieldbyname('srcacc').asinteger=acc_id) and
                  (btw(qka.fieldbyname('SrcType').asinteger,0,2)));
          prop:=((qka.fieldbyname('dstacc').asinteger=acc_id) and
                 (btw(qka.fieldbyname('DstType').asinteger,0,2)));
          showi:=(intop and vnb) or ((not intop) and ((raop and rab) or (prop and prb)));

          if qusr.Locate('id',qka.fieldbyname('user_cr').AsInteger,[]) then
              urcr:=qusr.fieldbyname('username').AsString else urcr:='<?>';

          if qusr.Locate('id',qka.fieldbyname('user_ch').AsInteger,[]) then
              urch:=qusr.fieldbyname('username').AsString else urch:='<?>';

          if showi then add_md_rec(qac,qcl,
                  qka.fieldbyname('kaid').asinteger, qka.fieldbyname('k_date').asdatetime,
                  qka.fieldbyname('prim').asstring,  qka.fieldbyname('opname').asstring,
                  qka.fieldbyname('bmk_id').asinteger, qka.fieldbyname('srcacc').asinteger,
                  qka.fieldbyname('dstacc').asinteger, qka.fieldbyname('SrcType').asinteger,
                  qka.fieldbyname('DstType').asinteger, qka.fieldbyname('Sn').ascurrency,
                  qka.fieldbyname('Sb').ascurrency, qka.fieldbyname('Su').ascurrency,
                  qka.fieldbyname('Dn').ascurrency, qka.fieldbyname('Db').ascurrency,
                  qka.fieldbyname('Du').ascurrency, urcr, urch,
                  qka.fieldbyname('r_cr').asstring); // SyncMod 29.04.2008

          qka.next;
      end;
    end;

    cucr:=colbyname('usrcr');
    if cucr<>nil then cucr.Visible:=n9.Checked;
    cucr:=colbyname('usrch');
    if cucr<>nil then cucr.Visible:=n9.Checked;

    dge1.visible:=md.recordcount>0;
    md.SortOnFields(_order);
    refreshsum;
    if _bmk>-1 then md.Locate('id',_bmk,[]) else md.first;
    md.EnableControls;
    fsq(qka);
    fsq(qcl);
    fsq(qac);
    fsq(qusr);
    sho.free;
    screen.Cursor:=crDefault;
end;

procedure TEAcc.N5Click(Sender: TObject);
begin
    n5.Checked:=not n5.checked;
    refresh_view;
end;

procedure TEAcc.N6Click(Sender: TObject);
begin
    n6.Checked:=not n6.checked;
    refresh_view;
end;

procedure TEAcc.N7Click(Sender: TObject);
begin
    n7.Checked:=not n7.checked;
    refresh_view;
end;

procedure TEAcc.N9Click(Sender: TObject);
begin
    n9.checked:=not n9.checked;
    refresh_view;
end;

procedure TEAcc.N3Click(Sender: TObject);
begin
    (sender as tmenuitem).checked:=true;
    view_option:=(sender as tmenuitem).tag;
    refresh_view;
end;

procedure TEAcc.LoadSettings;
var
    ini:tinifile;
begin
    ini:=tinifile.create(datadir+'\accounts.ini');
    view_option:=ini.readinteger(User.username+'/'+acc_title,'view_option',3);
    destart.Date:=ini.readdate(User.username+'/'+acc_title,'Start-date',now);
    deend.date:=ini.readdate(User.username+'/'+acc_title,'End-date',now);
    n5.checked:=ini.ReadBool(User.username+'/'+acc_title,'Prihod',true);
    n6.checked:=ini.readbool(User.username+'/'+acc_title,'Rashod',true);
    n7.Checked:=ini.readbool(User.username+'/'+acc_title,'Bookmarks',true);
    n8.checked:=ini.readbool(User.username+'/'+acc_title,'Internals',true);
    n9.checked:=ini.readbool(User.username+'/'+acc_title,'UserRecord',true);
    rsbfit.down:=ini.readbool(User.username+'/'+acc_title,'Fit',true);
    _order:=ini.readstring(User.username+'/'+acc_title,'Sort-order','date');
    dge1.RestoreColumnsLayout(ini,User.username+'/'+acc_title,[crpColIndexEh,crpColWidthsEh,crpSortMarkerEh]);
    ini.free;
end;

procedure TEAcc.SaveSettings;
var
    ini:tinifile;
begin
    ini:=tinifile.create(datadir+'\accounts.ini');
    ini.writeinteger(User.username+'/'+acc_title,'view_option',view_option);
    ini.WriteDate(User.username+'/'+acc_title,'Start-date',destart.date);
    ini.WriteDate(User.username+'/'+acc_title,'End-date',deend.date);
    ini.writebool(User.username+'/'+acc_title,'Prihod',n5.checked);
    ini.writebool(User.username+'/'+acc_title,'Rashod',n6.checked);
    ini.writebool(User.username+'/'+acc_title,'Bookmarks',n7.checked);
    ini.writebool(User.username+'/'+acc_title,'Internals',n8.checked);
    ini.writebool(User.username+'/'+acc_title,'UserRecord',n9.checked);
    ini.writebool(User.username+'/'+acc_title,'Fit',rsbfit.down);
    ini.writestring(User.username+'/'+acc_title,'Sort-order',_order);
    dge1.savecolumnslayout(ini,User.username+'/'+acc_title);
    ini.free;
end;

procedure TEAcc.Refresh_view;
begin
    refresh_controls;
    refresh_data;
end;

procedure TEAcc.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    SaveSettings;
    action:=cafree;
end;

procedure TEAcc.FormShow(Sender: TObject);
begin
    Loadsettings;
    Refresh_view;
end;

constructor TEAcc.Create(AOWner: TComponent; aacc_id: integer;
  aacc_name: string; aComRefProc: TComRefProc);
begin
    inherited Create(AOwner);
    FComRefProc:=aComRefProc;
    acc_id:=aacc_id;
    acc_title:=aacc_name;
    destart.date:=now;
    deend.date:=now;
end;

procedure TEAcc.rsbFitClick(Sender: TObject);
begin
    Refresh_controls;
end;

procedure TEAcc.rsbRefreshViewClick(Sender: TObject);
var
    dd:tdatetime;
begin
    if trunc(destart.date)>trunc(deend.date) then begin
        dd:=destart.date; destart.date:=deend.date; deend.date:=dd;
    end;
    refresh_view;
end;

procedure TEAcc.Do_Refresh_Account(jid: integer; aaction: integer);
var
    qusr, qac, qcl, q:tadsquery;
    intop, raop, prop, showi, din:boolean;
    lasta: integer;
    urcr, urch: string;
begin
    if (md.active) and (md.recordcount>0) then
        lasta:=md.fieldbyname('id').asinteger else lasta:=-1;
    case aaction of
        aact_delete: if md.Locate('id',jid,[]) then md.delete;
        aact_add,
        aact_modify:
        begin
            q:=adsq(datadir,'select '+
                    'ka.id kaid, op_id, prim, k_date, srcacc, dstacc, srctype, '+
                    'dsttype, sn, sb, su, sc, dn, db, du, dc, bmk_id, '+
                    'op.name opname, optype, user_cr, user_ch, r_cr from "'+
                    kas_name+'" ka, "'+ops_name+'" op where (op.id=ka.op_id) '+
                    'and (ka.id='+inttostr(jid)+')'); // SyncMod 29.04.2008
            if not emp(q) then begin
                din:=true;
                case view_option of
                    1: if not btw(q.fieldbyname('k_date').asdatetime, destart.date, deend.date) then din:=false;
                    3: if trunc(q.fieldbyname('k_date').asdatetime)<>trunc(destart.date) then din:=false;
                end;

                if din then begin

                    intop:=((q.fieldbyname('srcacc').asinteger=acc_id) and
                            (q.fieldbyname('srcacc').asinteger=
                             q.fieldbyname('dstacc').asinteger) and
                            (btw(q.fieldbyname('SrcType').asinteger,0,2)) and
                            (btw(q.fieldbyname('DstType').asinteger,0,2)));

                    raop:=((q.fieldbyname('srcacc').asinteger=acc_id) and
                            (btw(q.fieldbyname('SrcType').asinteger,0,2)));

                    prop:=((q.fieldbyname('dstacc').asinteger=acc_id) and
                           (btw(q.fieldbyname('DstType').asinteger,0,2)));

                    showi:=(intop and n8.checked) or
                          ((not intop) and ((raop and n6.checked) or (prop and n5.checked)));

                end else showi:=false;

                if aaction=aact_modify then
                    if md.locate('id',jid,[]) then md.delete;

                if showi then begin
                    qac:=adsq(datadir,'select * from "'+acc_name+'"');
                    qcl:=adsq(datadir,'select * from "'+cli_name+'"');
                    qusr:=adsq(datadir,'select * from "'+usr_name+'"');
                    if qusr.locate('id',q.fieldbyname('user_cr').AsInteger,[]) then
                        urcr:=qusr.fieldbyname('username').asstring else urcr:='<?>';
                    if qusr.locate('id',q.fieldbyname('user_ch').AsInteger,[]) then
                        urch:=qusr.fieldbyname('username').asstring else urch:='<?>';
                    md.DisableControls;
                    add_md_rec(qac,qcl,
                               q.fieldbyname('kaid').asinteger,
                               q.fieldbyname('k_date').asdatetime,
                               q.fieldbyname('prim').asstring,
                               q.fieldbyname('opname').asstring,
                               q.fieldbyname('bmk_id').asinteger,
                               q.fieldbyname('srcacc').asinteger,
                               q.fieldbyname('dstacc').asinteger,
                               q.fieldbyname('SrcType').asinteger,
                               q.fieldbyname('DstType').asinteger,
                               q.fieldbyname('Sn').ascurrency,
                               q.fieldbyname('Sb').ascurrency,
                               q.fieldbyname('Su').ascurrency,
                               q.fieldbyname('Dn').ascurrency,
                               q.fieldbyname('Db').ascurrency,
                               q.fieldbyname('Du').ascurrency,
                               urcr, urch,
                               q.fieldbyname('r_cr').AsString); // SyncMod 29.04.2008
                    dge1SortMarkingChanged(dge1);
                    md.locate('id',jid,[]);
                    md.EnableControls;
                    addd(q.fieldbyname('k_date').asdatetime);
                    Fill_days;
                    fsq(qac);
                    fsq(qcl);
                    fsq(qusr);
                end;
            end;
            fsq(q);
        end;
    end;
    if lasta>-1 then begin
        refreshsum;
        md.locate('id',lasta,[]);
    end;
    dge1.visible:=(md.active) and (md.recordcount>0);
end;

function TEAcc.Check_user_rights(jid: integer): string;
var
    zq: tadsquery;
begin
    result:='';
    zq:=adsq(datadir, format('select * from "%s" where ID=%d', [kas_name, jid]));
    if emp(zq) then result:=result+'* Указанная запись отсутствует'#13
    else begin
        if (zq.FieldByName('dsttype').AsInteger in [0..2]) and
           (not user.Account[zq.FieldByName('dstacc').AsInteger].op_in) then
            result:=result+'* Ввод средств на счёт выбранной записи'#13;
        if (zq.FieldByName('srctype').AsInteger in [0..2]) and
           (not user.Account[zq.FieldByName('srcacc').AsInteger].op_out) then
            result:=result+'* Вывод средств со счёта выбранной записи'#13;
        if not user.Op[zq.FieldByName('op_id').AsInteger].en then
            result:=result+'* Нет прав на кассовую операцию'#13;
    end;
    fsq(zq);
end;

procedure TEAcc.Do_Modify_Record(jid: integer); //MU OK
var
    er: string;
begin
    er:=Check_user_rights(jid);
    if er='' then begin
        eact.jid:=jid;
        if EAct.showmodal=mrok then
            if assigned(FComRefProc) then
                FComRefProc(jid,aact_modify);
    end else showmessage('Для изменения записи недостаточно прав:'#13#13+er);
end;

procedure TEAcc.LogDeletedRecord(aID: integer);
const
  cacctype: array[0..3] of string = ('Наличные', 'Безналичные', 'Валюта', 'Контрагент');
var
  q, qcli, qop, qacc, qusr: tadsquery;
  f: TextFile;
  dfe: boolean;
  sl: TStringList;
  s1, s2: string;
begin
  q := adsq(datadir, format('SELECT * FROM "kassa.adt" WHERE id=%d', [aID]));
  if not emp(q) then
  begin
    qcli := adsq(datadir, 'SELECT * FROM "clients.adt"');
    qop := adsq(datadir, 'SELECT * FROM "operations.adt"');
    qacc := adsq(datadir, 'SELECT * FROM "accounts.adt"');
    qusr := adsq(datadir, 'SELECT * FROM "users.adt"');
    sl := TStringList.Create;
    sl.Clear;
    sl.Add('***');
    sl.Add(format('Дата удаления: %s %s', [datetostr(now), timetostr(now)]));
    sl.Add(' ');

    sl.Add(format('Дата создания записи: %s', [
      DateToStr(q.fieldbyname('k_date').AsDateTime)])
    );

    sl.Add(format('Сумма: %s (Курс: %s)', [formatfloat('#####0.00',
      q.fieldbyname('k_sum').AsCurrency), formatfloat('#####0.00',
      q.fieldbyname('k_usd').AsCurrency)])
    );

    if (not emp(qop)) and (qop.Locate('ID', q.fieldbyname('op_id').AsInteger, [])) then
      sl.add(format('Операция: %s', [qop.FieldByName('Name').AsString]))
    else
      sl.Add('Операция: неизвестно');


// =====
    try
      s2 := cacctype[q.FieldByName('SrcType').AsInteger];
    except
      s2 := 'Неизвестно';
    end;

    if (q.FieldByName('SrcType').AsInteger in [0..2]) then
    begin
      if qacc.Locate('ID', q.fieldbyname('SrcAcc').AsInteger, []) then
        s1 := qacc.fieldbyname('Name').AsString
      else
        s1 := 'Неизвестный счёт';
    end
    else
    begin
      if qcli.Locate('ID', q.fieldbyname('SrcAcc').AsInteger, []) then
        s1 := qcli.fieldbyname('c_name').AsString
      else
        s1 := 'Неизвестный контрагент';
    end;

    sl.Add(format('Источник: %s (%s)', [s1, s2]));
// =====
    try
      s2 := cacctype[q.FieldByName('DstType').AsInteger];
    except
      s2 := 'Неизвестно';
    end;

    if (q.FieldByName('DstType').AsInteger in [0..2]) then
    begin
      if qacc.Locate('ID', q.fieldbyname('DstAcc').AsInteger, []) then
        s1 := qacc.fieldbyname('Name').AsString
      else
        s1 := 'Неизвестный счёт';
    end
    else
    begin
      if qcli.Locate('ID', q.fieldbyname('DstAcc').AsInteger, []) then
        s1 := qcli.fieldbyname('c_name').AsString
      else
        s1 := 'Неизвестный контрагент';
    end;

    sl.Add(format('Приёмник: %s (%s)', [s1, s2]));
//=====

    if trim(q.fieldbyname('prim').AsString) <> '' then
      sl.Add(format('Примечание: %s', [q.fieldbyname('prim').AsString]));

    if (not emp(qusr)) and (qusr.Locate('ID', q.fieldbyname('user_cr').AsInteger, [])) then
      sl.Add(format('Запись создал: %s', [qusr.FieldByName('username').AsString]))
    else
      sl.add('Запись создал: Неизвестно');

    if (not emp(qusr)) and (qusr.Locate('ID', q.fieldbyname('user_ch').AsInteger, [])) then
      sl.Add(format('Запись изменил: %s', [qusr.FieldByName('username').AsString]))
    else
      sl.add('Запись изменил: Неизвестно');

    if (not emp(qusr)) and (qusr.Locate('ID', user.userid, [])) then
      sl.Add(format('Запись удалил: %s', [qusr.FieldByName('username').AsString]))
    else
      sl.add('Запись удалил: Неизвестно');

    dfe := FileExists(datadir + '\DeletedLog.txt');
    AssignFile(f, datadir + '\DeletedLog.txt');
    if dfe then
      Append(f)
    else
      Rewrite(f);

    try
      while sl.Count > 0 do
      begin
        writeln(f, sl[0]);
        sl.Delete(0);
      end;
    except
    end;

    CloseFile(f);
    fsq(qcli);
    fsq(qop);
    fsq(qacc);
    fsq(qusr);
  end;

  fsq(q);
end;

procedure TEAcc.Do_Delete_Record(jid, abmk: integer; ats: string); //MU OK // SyncMod 29.04.2008
var
    er: string;
begin
    er:=Check_user_rights(jid);
    if not user.delrecords then
      er := er + '* Недостаточно прав для удаления записей'#13;
    if er='' then begin
        if dlg('Удаление записи','Вы уверены в том, что хотите удалить запись?',
               'Да|Нет',nil)=0 then begin
            LogDeletedRecord(jid);
// SyncMod 29.04.2008
            SetDeletedRecord(User.userid, ats, kas_name);
// SyncMod /
            adse(datadir,'delete from "'+kas_name+'" where id='+inttostr(jid));

            if abmk>-1 then adse(datadir,'delete from "'+bmk_name+'" where id='+inttostr(abmk));
            if assigned(FComRefProc) then
                FComRefProc(jid,aact_delete);
        end;
    end else showmessage('Для удаления записи недостаточно прав:'#13#13+er);
end;

procedure TEAcc.dge1KeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    case key of
        vk_delete:
            if (md.Active) and (md.recordcount>0) then
                Do_Delete_Record(md.fieldbyname('id').AsInteger,
                    md.fieldbyname('bmk_id').AsInteger,
                    md.fieldbyname('r_cr').AsString); // SyncMod 29.04.2008
        vk_space:
            if (md.Active) and (md.recordcount>0) then
                Do_Modify_Record(md.fieldbyname('id').AsInteger);
    end;
    // MACDELETE
    if (ssCtrl in Shift) and (Key = $44) then
    begin
      if (md.Active) and (md.recordcount>0) then
        Do_Delete_Record(md.fieldbyname('id').AsInteger,
          md.fieldbyname('bmk_id').AsInteger,
          md.fieldbyname('r_cr').AsString); // SyncMod 29.04.2008
    end;
    // MACDELETE /
end;

procedure TEAcc.dge1SortMarkingChanged(Sender: TObject);
var
    f:integer;
    sf: string;
    desc: boolean;
begin
    sb.SimpleText:='Сортировка...';
    sf:='';
    desc:=false;
    for f:=0 to dge1.SortMarkedColumns.Count-1 do begin
        desc:=(dge1.SortMarkedColumns[f].Title.SortMarker = smUpEh);
        if sf<>'' then sf:=sf+';';
        sf:=sf+dge1.SortMarkedColumns[f].FieldName;
    end;
    if sf<>'' then begin
        md.disablecontrols;
        md.SortOnFields(sf,true,desc);
        md.enablecontrols;
    end;
    Refresh_controls;
    sb.simpletext:='';
end;

procedure TEAcc.dge1DblClick(Sender: TObject);
begin
    if (md.active) and (md.recordcount>0) then
        Do_Modify_Record(md.fieldbyname('id').AsInteger);
end;

function TEAcc.colbyname(n:string):tcolumneh;
var
    f:integer;
begin
    result:=nil;
    if dge1.Columns.Count>0 then
        for f:=0 to dge1.columns.count-1 do
            if ansisametext(n,dge1.columns[f].FieldName) then begin
                result:=dge1.Columns.Items[f];
                exit;
            end;
end;

procedure TEAcc.refreshsum;
var
    b:integer;
    anp,anr,abp,abr,aup,aur:currency;
begin
    if md.RecordCount>0 then begin
        anp:=0; anr:=0; abp:=0; abr:=0; aup:=0; aur:=0;
        md.DisableControls;
        b:=md.fieldbyname('id').asinteger;
        md.First;
        while not md.Eof do begin
            anp:=anp+md.fieldbyname('Np').ascurrency;
            anr:=anr+md.fieldbyname('Nr').ascurrency;
            abp:=abp+md.fieldbyname('Bp').ascurrency;
            abr:=abr+md.fieldbyname('Br').ascurrency;
            aup:=aup+md.fieldbyname('Up').ascurrency;
            aur:=aur+md.fieldbyname('Ur').ascurrency;
            md.Next;
        end;
        colbyname('Np').Footers[0].Value:=formatfloat('# ### ### ##0.00',anp);
        colbyname('Nr').Footers[0].Value:=formatfloat('# ### ### ##0.00',anr);
        colbyname('Bp').Footers[0].Value:=formatfloat('# ### ### ##0.00',abp);
        colbyname('Br').Footers[0].Value:=formatfloat('# ### ### ##0.00',abr);
        colbyname('Up').Footers[0].Value:=formatfloat('# ### ### ##0.00',aup);
        colbyname('Ur').Footers[0].Value:=formatfloat('# ### ### ##0.00',aur);
        colbyname('Nr').Footers[1].Value:=formatfloat('# ### ### ##0.00',anp-anr);
        colbyname('Br').Footers[1].Value:=formatfloat('# ### ### ##0.00',abp-abr);
        colbyname('Ur').Footers[1].Value:=formatfloat('# ### ### ##0.00',aup-aur);
        md.locate('id',b,[]);
        md.EnableControls;
    end;
end;

procedure TEAcc.deStartAcceptDate(Sender: TObject; var ADate: TDateTime;
  var Action: Boolean);
begin
    if view_option=3 then begin
        deStart.date:=ADate;
        refresh_view;
    end;
end;

procedure TEAcc.aPrintAccExecute(Sender: TObject);
var
    tts: string;
begin
    if dge1.visible then begin
        pdb.Title.Clear;
        pdb.title.add(caption);
        tts:='';
        case view_option of
            1: tts:='Записи с '+longdatetostr(destart.date)+' по '+longdatetostr(deend.date);
            2: tts:='Все записи счёта';
            3: tts:='Дата: '+longdatetostr(destart.date);
        end;
        pdb.title.add(tts);
        tts:='';
        if label4.visible then tts:=tts+'<Приход> ';
        if label5.visible then tts:=tts+'<Расход> ';
        if label7.visible then tts:=tts+'<Операции внутри счёта>';
        pdb.title.add('Типы операций: '+tts);
        pdb.preview;
    end;
end;

procedure TEAcc.N8Click(Sender: TObject);
begin
    n8.Checked:=not n8.checked;
    refresh_view;
end;

procedure TEAcc.dge1GetCellParams(Sender: TObject; Column: TColumnEh;
  AFont: TFont; var Background: TColor; State: TGridDrawState);
const
    cl:array[false..true, false..true] of tcolor=((clWhite,clBlack),($c0ffc0,clBlack));
    cf:array[false..true, false..true] of tcolor=((clBlack,clWhite),(clBlack,$c0ffc0));

var
    hl: boolean;
begin
    hl:=(state=[gdselected]) or (state=[gdselected,gdfocused]);
    if column.Field.DataSet.fieldbyname('bmk_id').AsInteger>-1 then begin
        background:=cl[n7.checked,hl];
        aFont.color:=cf[n7.checked,hl];
    end;
end;

function TEAcc.Get_Pacient(aAcc,aType: integer):string;
var
    q:tadsquery;
begin
    result:='?';
    q:=nil;
    case aType of
        0..2: q:=adsq(datadir,'select name hname from "'+acc_name+'" where id='+inttostr(aAcc));
        3: q:=adsq(datadir,'select C_Name hname from "'+cli_name+'" where id='+inttostr(aAcc));
    end;
    if q<>nil then begin
        if (q.active) and (q.recordcount>0) then result:=q.fieldbyname('hname').asstring;
        q.close;
        q.free;
    end;
end;

function TEAcc.Get_Passport(aAcc,aType: integer):string;
var
    u:tadsquery;
    sl:tstringlist;
begin
    result:='';
    u:=nil;
    case aType of
    0,1,2: begin
            u:=adsq(datadir,'select Data from "'+acc_name+'" where id='+inttostr(aAcc));
            if (u<>nil) and (u.active) and (u.recordcount>0) then begin
                sl:=tstringlist.create;
                sl.text:=u.fieldbyname('data').AsString;
                result:=getvalue(sl,'Passport','');
                sl.free;
            end;
           end;
    3:     begin
            u:=adsq(datadir,'select * from "'+cli_name+'" where id='+inttostr(aAcc));
            if (u<>nil) and (u.active) and (u.recordcount>0) then
                result:=u.fieldbyname('c_passport').AsString;
           end;
    end;
    if u<>nil then begin
        u.close;
        u.free;
    end;
end;

procedure TEAcc.DeleteOrders(jid, doctype: integer);
var
  q: tadsquery;
begin
  q := adsq(datadir, 'select * from "' + ord_name + '" where (jid=' +
    inttostr(jid) + ') and (doctype=' + inttostr(doctype) + ')');
  if not emp(q) then
  begin
    while not q.Eof do
    begin
      SetDeletedRecord(User.userid, q.fieldbyname('r_cr').AsString, ord_name);
      q.Next;
    end;
  end;
  fsq(q);
end;

procedure TEAcc.MakeRashodOrder(jid: integer);
const
    dsm:array[0..3] of string=('sn','sb','su','sc');
var
    sl:tstringlist;
    accid,DocNum: integer;
    DocDate: tdatetime;
    ts, Src,Dst,Subj,Passport: string;
    DocSum: currency;
    DocUsd: boolean;
    qc, q,u:tadsquery;

begin
    q:=adsq(datadir,'select * from "'+kas_name+'" where id='+inttostr(jid));
    if q<>nil then begin
        if (q.active) and (q.recordcount>0) then begin
            passport:='';
            AccID:=q.fieldbyname('SrcAcc').AsInteger;
            DocDate:=q.fieldbyname('K_Date').asdatetime;
            //DocType=0
            Src:=Get_Pacient(q.fieldbyname('SrcAcc').AsInteger,q.fieldbyname('SrcType').AsInteger);
            Dst:=Get_Pacient(q.fieldbyname('DstAcc').AsInteger,q.fieldbyname('DstType').AsInteger);
            Subj:=q.fieldbyname('Prim').AsString;
            DocSum:=q.fieldbyname(dsm[q.fieldbyname('SrcType').asinteger]).AsCurrency;
            DocUsd:=q.fieldbyname('SrcType').AsInteger=2;
            Passport:=get_passport(q.fieldbyname('DstAcc').AsInteger,q.fieldbyname('DstType').AsInteger);

            u:=adsq(datadir,'select * from "'+ord_name+'" where (jid='+inttostr(jid)+
                    ') and (DocType=0)');
            if (u<>nil) and (u.active) and (u.recordcount>0) then begin
                DocNum:=u.fieldbyname('DocNum').AsInteger;
                if trim(u.fieldbyname('passport').AsString)<>'' then
                    passport:=u.fieldbyname('passport').asstring;
            end else
                DocNum:=dm.OrderNextNum(accid,0);
            if u<>nil then begin
                u.close;
                u.free;
            end;
            if trim(passport)='' then begin
                case q.fieldbyname('DstType').AsInteger of
                0,1,2: begin
                            u:=adsq(datadir,'select * from "'+acc_name+'" where ID='+
                                    inttostr(q.fieldbyname('DstAcc').AsInteger));
                            if (u<>nil) then begin
                                if (u.active) and (u.recordcount>0) then begin
                                    sl:=tstringlist.create;
                                    sl.text:=u.fieldbyname('data').AsString;
                                    passport:=getvalue(sl,'Passport','');
                                    if trim(passport)='' then begin
                                        eagentdic.eAgent.Text:=getvalue(sl,'Agent','');
                                        eagentdic.ePassport.Text:=getvalue(sl,'Passport','');
                                        if eagentdic.ShowModal=mrok then begin
                                            setvalue(sl,'Agent',eagentdic.eAgent.Text);
                                            setvalue(sl,'Passport',eagentdic.ePassport.Text);

                                            adse(datadir,'update "'+acc_name+'" set Data='+
                                                quotedstr(sl.Text)+', r_ch=' +
                                                quotedstr(GetTimeStamp) + ' where id='+
                                                inttostr(q.fieldbyname('DstAcc').AsInteger));
                                        end;
                                    end;
                                    sl.free;
                                end;
                                u.close;
                                u.free;
                            end;
                       end;
                3:     begin
                            u:=adsq(datadir,'select * from "'+cli_name+'" where id='+
                                inttostr(q.fieldbyname('DstAcc').AsInteger));
                            if (u<>nil) then begin
                                if (u.active) and (u.recordcount>0) then
                                    Passport:=u.FieldByName('c_passport').asstring;
                                u.close;
                                u.free;
                            end;
                            if trim(passport)='' then begin
                                qc:=adsq(datadir,format('select * from "%s" where ID=%d',
                                    [cli_name,q.fieldbyname('DstAcc').AsInteger]));
                                if not emp(qc) then begin
                                    ecln.id:=qc.fieldbyname('ID').AsInteger;
                                    ecln.eName.Text:=qc.fieldbyname('c_name').asstring;
                                    ecln.ePassport.Text:=qc.fieldbyname('c_passport').asstring;
                                    ecln.rleGrp.text:=qc.fieldbyname('c_group').AsString;

                                    if ecln.ShowModal=mrok then begin
                                        adse(datadir,'update "'+cli_name+'" set c_name='+
                                             quotedstr(ecln.ename.text)+', c_passport='+
                                             quotedstr(ecln.epassport.text)+
                                             ' where id='+inttostr(ecln.id));
                                        Dst:=ecln.ename.text;
                                        Passport:=ecln.epassport.text;
                                    end;
                                end else showmessage('Внутренняя ошибка');
                                fsq(qc);
                            end;
                       end;

                end;
            end;
            DeleteOrders(jid, 0);
            adse(datadir,'delete from "'+ord_name+'" where (jid='+inttostr(jid)+
                 ') and (DocType=0)');
            ts := quotedstr(GetTimeStamp);
            adse(datadir,'insert into "'+ord_name+
                 '" (jid, accid, DocNum, DocDate, DocType, Src, Dst, Subj, ' +
                 'DocSum, DocUsd, Passport, r_cr, r_ch) values ('+
                 inttostr(jid)+', '+inttostr(accid)+', '+inttostr(DocNum)+', '+
                 quotedstr(datetostr(DocDate))+', 0, '+quotedstr(src)+', '+
                 quotedstr(dst)+', '+quotedstr(subj)+', '+f2s(DocSum)+', '+
                 yn[DocUsd]+', '+quotedstr(Passport)+ ', '+ts+', '+ts+')');
        end;
        q.close;
        q.Free;
    end;
end;

procedure TEAcc.MakePrihodOrder(jid: integer);
const
    dsm:array[0..3] of string=('dn','db','du','dc');
var
    accid,DocNum: integer;
    DocDate: tdatetime;
    ts, Src,Dst,Subj: string;
    DocSum: currency;
    DocUsd: boolean;
    q,u:tadsquery;
begin
    q:=adsq(datadir,'select * from "'+kas_name+'" where id='+inttostr(jid));
    if q<>nil then begin
        if (q.active) and (q.recordcount>0) then begin
            AccID:=q.fieldbyname('DstAcc').AsInteger;
            DocDate:=q.fieldbyname('K_Date').asdatetime;
            //DocType=0
            Src:=Get_Pacient(q.fieldbyname('SrcAcc').AsInteger,q.fieldbyname('SrcType').AsInteger);
            Dst:=Get_Pacient(q.fieldbyname('DstAcc').AsInteger,q.fieldbyname('DstType').AsInteger);
            Subj:=q.fieldbyname('Prim').AsString;
            DocSum:=q.fieldbyname(dsm[q.fieldbyname('DstType').asinteger]).AsCurrency;
            DocUsd:=q.fieldbyname('DstType').AsInteger=2;

            u:=adsq(datadir,'select * from "'+ord_name+'" where (jid='+inttostr(jid)+
                    ') and (DocType=1)');
            if (u<>nil) and (u.active) and (u.recordcount>0) then 
                DocNum:=u.fieldbyname('DocNum').AsInteger
                else DocNum:=dm.OrderNextNum(accid,0);
            if u<>nil then begin
                u.close;
                u.free;
            end;
            DeleteOrders(jid, 1);
            adse(datadir,'delete from "'+ord_name+'" where (jid='+inttostr(jid)+
                 ') and (DocType=1)');
            ts := quotedstr(GetTimeStamp);
            adse(datadir,'insert into "'+ord_name+
                 '" (jid, accid, DocNum, DocDate, DocType, Src, Dst, Subj, ' +
                 'DocSum, DocUsd, r_cr, r_ch) values ('+
                 inttostr(jid)+', '+inttostr(accid)+', '+inttostr(DocNum)+', '+
                 quotedstr(datetostr(DocDate))+', 1, '+quotedstr(src)+', '+
                 quotedstr(dst)+', '+quotedstr(subj)+', '+f2s(DocSum)+', '+
                 yn[DocUsd]+', '+ts+', '+ts+')');
        end;
        q.close;
        q.Free;
    end;
end;

procedure TEAcc.aOrderExecute(Sender: TObject);
var
    vjid,rps:integer;
    q:tadsquery;
begin
    if (not md.active) or (md.recordcount<1) then exit;
    rps:=0;
    vjid:=md.fieldbyname('ID').AsInteger;
    q:=adsq(datadir,'select * from "'+kas_name+'" where id='+inttostr(vjid));
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            //vdoctype:
            //  Расход=0
            //  Приход=1
            if (q.fieldbyname('SrcType').asinteger in [0..2]) and
               (q.fieldbyname('SrcAcc').asinteger=Acc_ID) then rps:=rps+1;
            if (q.fieldbyname('DstType').asinteger in [0..2]) and
               (q.fieldbyname('DstAcc').asinteger=Acc_ID) then rps:=rps+2;
        end;
        q.close;
        q.free;
    end;
    case rps of
        1: begin
            makeRashodOrder(vjid);
            dm.PrintOrder(vjid,0,false);
           end; 
        2: begin
            makePrihodOrder(vjid);
            dm.PrintOrder(vjid,1,false);
           end;
        3: case dlg('Два документа',
                    'Эта операция является перемещением внутри счёта.'#13+
                    'Пожалуйста, выберите нужный документ:',
                    'Расходный ордер|Приходный ордер',nil) of
               0: begin
                    makeRashodOrder(vjid);
                    dm.PrintOrder(vjid,0,false);
                  end;
               1: begin
                    makePrihodOrder(vjid);
                    dm.PrintOrder(vjid,1,false);
                  end;
           end;
    end;
end;

destructor TEAcc.Destroy;
begin
  inherited Destroy;
  Form1.Do_windows_menu;
end;



procedure TEAcc.aIntOrderExecute(Sender: TObject);
var
    vjid,rps:integer;
    q:tadsquery;
begin
    if (not md.active) or (md.recordcount<1) then exit;
    rps:=0;
    vjid:=md.fieldbyname('ID').AsInteger;
    q:=adsq(datadir,'select * from "'+kas_name+'" where id='+inttostr(vjid));
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            //vdoctype:
            //  Расход=0
            //  Приход=1
            if (q.fieldbyname('SrcType').asinteger in [0..2]) and
               (q.fieldbyname('SrcAcc').asinteger=Acc_ID) then rps:=rps+1;
            if (q.fieldbyname('DstType').asinteger in [0..2]) and
               (q.fieldbyname('DstAcc').asinteger=Acc_ID) then rps:=rps+2;
        end;
        q.close;
        q.free;
    end;
    case rps of
        1,3: begin
            makeRashodOrder(vjid);
            dm.PrintOrder(vjid,0,true);
           end;
    end;
end;

procedure TEAcc.rsbExpCsvClick(Sender: TObject);
begin
  dm.SaveDbgToCsv(dge1);
end;

end.
