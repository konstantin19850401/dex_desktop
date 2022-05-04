unit FESvParam;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, RXCtrls, Mask, ToolEdit, ComCtrls, sysrelate, DB,
  adsdata, adsfunc, adstable, adsrelate;

type
  TESvParam = class(TForm)
    Label1: TLabel;
    eTitle: TEdit;
    pc: TPageControl;
    ts1: TTabSheet;
    ts2: TTabSheet;
    GroupBox1: TGroupBox;
    deStart: TDateEdit;
    deend: TDateEdit;
    GroupBox2: TGroupBox;
    RxSpeedButton1: TRxSpeedButton;
    RxSpeedButton2: TRxSpeedButton;
    clbOP: TRxCheckListBox;
    clbcli: TRxCheckListBox;
    clbacc: TRxCheckListBox;
    Label2: TLabel;
    Label3: TLabel;
    cbGrp: TComboBox;
    Label4: TLabel;
    cbRestrictAcc: TCheckBox;
    procedure RxSpeedButton2Click(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure RxSpeedButton1Click(Sender: TObject);
    procedure cbGrpChange(Sender: TObject);
    procedure clbcliKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure clbaccKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure clbOPKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
  private
    procedure SetChecks(aclb: trxchecklistbox; src: string);
    function GetChecks(aclb: trxchecklistbox): string;
    procedure Prepare_Grp;
    procedure Prepare_cln;
    procedure UpClb(aclb: trxchecklistbox; key: word);
    { Private declarations }
  public
    { Public declarations }
    SvData: string;
    procedure LoadSvodka(d: string);
  end;

var
  ESvParam: TESvParam;

implementation

uses Unit2;

{$R *.dfm}
{Формат сводки:

Title=Наименование сводки
Option=тип отчёта. 0=контрагенты; 1=счета
ClientGroup=Группа контрагентов. Если не указано - все группы.
ClientList=список контрагентов (через ;)
AccountList=список счетов (через ;)
Oplist=список операций (через ;)
DateBegin=начальная дата
DateEnd=конечная дата

Пример сводки:

Title=Зарплата за январь
Option=0
ClientGroup=Субдилеры
ClientList=1;2;3;4
AccountList=1;2
Oplist=1;2;3
DateBegin=01.02.2003
DateEnd=28.02.2003
}

procedure TESvParam.SetChecks(aclb: trxchecklistbox; src: string);
var
    ps:pparsedstr;
    f,g:integer;
    i:array of integer;
begin
    ps:=parsestring(src,';');
    setlength(i,ps.count);
    if ps.count>0 then begin
        for f:=0 to ps.count-1 do
            try i[f]:=strtoint(ps.sa[f]);
            except i[f]:=-1;
            end;
        if aclb.Items.Count>0 then
            for f:=0 to aclb.Items.Count-1 do
                for g:=0 to ps.count-1 do
                    if integer(aclb.Items.Objects[f])=i[g] then aclb.Checked[f]:=true;
    end;
    setlength(i,0);
    freeparsedstr(ps);
end;

function TESvParam.GetChecks(aclb: trxchecklistbox): string;
var
    f:integer;
begin
    result:='';
    if aclb.Items.Count>0 then
        for f:=0 to aclb.Items.Count-1 do
            if aclb.Checked[f] then begin
                if result<>'' then result:=result+';';
                result:=result+inttostr(integer(aclb.Items.Objects[f]));
            end;
end;

procedure TESvParam.LoadSvodka(d:string);
var
    sl:tstringlist;
    svtype: integer;
begin
    sl:=tstringlist.create;
    sl.Text:=d;
    eTitle.text:=getvalue(sl,'Title','Сводка');
    try svtype:=strtoint(getvalue(sl,'Option','0'))
    except svtype:=0;
    end;
    pc.ActivePageIndex:=svtype;
    case svtype of
        0: begin
            if trim(getvalue(sl,'ClientGroup',''))<>'' then
                cbgrp.ItemIndex:=cbgrp.Items.IndexOf(getvalue(sl,'ClientGroup',''))
                else cbgrp.ItemIndex:=0;
            if cbgrp.itemindex<0 then cbgrp.itemindex:=0;
            prepare_cln;
            setchecks(clbcli,getvalue(sl,'ClientList',''));
            cbRestrictAcc.Checked:=ansisametext(getvalue(sl,'RestrictAcc',yn[false]),yn[true]);
            if cbrestrictacc.checked then
                setchecks(clbacc,getvalue(sl,'AccountList',''));
           end;
        1: setchecks(clbacc,getvalue(sl,'AccountList',''));
    end;
    setchecks(clbop,getvalue(sl,'Oplist',''));
    try deStart.Date:=strtodate(getvalue(sl,'DateBegin',datetostr(now)));
    except end;
    try deEnd.Date:=strtodate(getvalue(sl,'DateEnd',datetostr(now)));
    except end;
    sl.free;
end;

procedure TESvParam.RxSpeedButton2Click(Sender: TObject);
begin
    modalresult:=mrcancel;
end;

procedure TESvParam.Prepare_Grp;
var
    q:tadsquery;
begin
    cbgrp.Items.Clear;
    cbgrp.Items.Add('Все группы');
    q:=adsq(datadir,'select distinct c_group from "'+cli_name+'" order by c_group');
    if q<>nil then begin
        if (q.active) and (q.recordcount>0) then begin
            q.first;
            while not q.Eof do begin
                if trim(q.fieldbyname('c_group').AsString)<>'' then
                    cbgrp.Items.Add(q.fieldbyname('c_group').AsString);
                q.Next;
            end;
        end;
        q.Close;
        q.free;
    end;
end;

procedure TESvParam.Prepare_cln;
var
    q:tadsquery;
    wh:string;
    i:integer;
begin
    clbcli.Clear;
    if cbgrp.ItemIndex>0 then wh:=' where c_group='+quotedstr(cbgrp.Items[cbgrp.itemindex])
    else wh:='';
    q:=adsq(datadir,'select * from "'+cli_name+'"'+wh+' order by c_name');
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            q.first;
            while not q.Eof do begin
                i:=clbcli.Items.add(q.fieldbyname('c_name').AsString);
                clbcli.Items.Objects[i]:=pointer(q.fieldbyname('id').AsInteger);
                q.Next;
            end;
        end;
        q.Close;
        q.free;
    end;
end;

procedure TESvParam.FormShow(Sender: TObject); //MU OK
const
    opt:array[false..true] of string=('Р','П');
var
    q:tadsquery;
    i:integer;
begin
//Клиенты
    Prepare_grp;
    cbgrp.itemindex:=0;
    prepare_cln;
//Счета
    clbacc.Clear;
    q:=adsq(datadir, format('select * from "%s" where id IN (%s) order by name',
        [acc_name, user.GetAccountsIN(AIN_BOTH)]));
//    q:=adsq(datadir,'select * from "'+acc_name+'" order by name');
    if not emp(q) then begin
        q.first;
        while not q.Eof do begin
            i:=clbacc.Items.add(q.fieldbyname('name').AsString);
            clbacc.Items.Objects[i]:=pointer(q.fieldbyname('id').AsInteger);
            q.Next;
        end;
    end;
    fsq(q);

//Операции
    clbop.Clear;
    q:=adsq(datadir, format('select * from "%s" where id IN (%s) order by optype, name',
        [ops_name, user.GetOpsIN]));
//    q:=adsq(datadir,'select * from "'+ops_name+'" order by optype,name');
    if not emp(q) then begin
        q.first;
        while not q.Eof do begin
            i:=clbop.Items.add('['+opt[q.fieldbyname('optype').asboolean]+'] '+
                                q.fieldbyname('name').AsString);
            clbop.Items.Objects[i]:=pointer(q.fieldbyname('id').AsInteger);
            q.Next;
        end;
    end;
    fsq(q);
    
//Прочие данные
    pc.ActivePageIndex:=0;
    eTitle.Text:='Сводка';
    deStart.Date:=now;
    deend.date:=now;
    if trim(svdata)<>'' then loadsvodka(svdata);
end;

procedure TESvParam.RxSpeedButton1Click(Sender: TObject);
const
    svts:array[0..1] of string=('контрагентам','счетам');
var
    er: string;
    cgcs, accs, clis, opss: string;
begin
    er:='';
    try strtodate(destart.Text);
    except er:=er+'Некорректная начальная дата периода'#13;
    end;
    try strtodate(deend.Text);
    except er:=er+'Некорректная конечная дата периода'#13;
    end;
    if trunc(destart.date)>trunc(deend.date) then
        er:=er+'Начальная дата не может быть позже конечной'#13;
    accs:=getchecks(clbacc);
    clis:=getchecks(clbcli);
    opss:=getchecks(clbop);
    case pc.ActivePageIndex of
        0: begin
            if trim(clis)='' then er:=er+'Не указано ни одного контрагента в качестве критерия для отбора'#13;
            if (cbRestrictAcc.Checked) and (trim(accs)='') then
                er:=er+'Не указано ни одного счёта в качестве ограничителя отбора'#13;
           end;
        1: if trim(accs)='' then er:=er+'Не указано ни одного счёта в качестве критерия для отбора'#13;
    end;
    if trim(opss)='' then er:=er+'Не указано ни одной операции в качестве критерия для отбора'#13;
    if er='' then begin
        if trim(etitle.text)='' then
            eTitle.text:='Сводка: '+datetostr(destart.date)+' - '+
                         datetostr(deend.date)+' по '+svts[pc.ActivePageindex];
        if cbgrp.ItemIndex<1 then cgcs:='' else cgcs:=cbgrp.Items[cbgrp.itemindex];
        svdata:=
            'Title='+eTitle.Text+#13+
            'Option='+inttostr(pc.ActivePageIndex)+#13+
            'ClientGroup='+cgcs+#13+
            'ClientList='+clis+#13+
            'AccountList='+accs+#13+
            'Oplist='+opss+#13+
            'RestrictAcc='+yn[cbrestrictacc.checked]+#13+
            'DateBegin='+datetostr(destart.date)+#13+
            'DateEnd='+datetostr(deend.date);
        modalresult:=mrok;
    end else showmessage('Ошибки:'#13#13+er);
end;

procedure TESvParam.cbGrpChange(Sender: TObject);
var
    src:string;
begin
    src:=getchecks(clbcli);
    prepare_cln;
    setchecks(clbcli,src);
end;

procedure TEsvParam.UpClb(aclb: trxchecklistbox; key: word);
const
    sni: array[0..1] of boolean=(false,true);
var
    ssf,f:integer;
begin
    case key of
        vk_f12: ssf:=1;
        vk_f11: ssf:=0;
        else ssf:=-1;
    end;
    if (ssf>-1) and (aclb.Items.Count>0) then
        for f:=0 to aclb.items.count-1 do
            aclb.Checked[f]:=sni[ssf];
end;

procedure TESvParam.clbcliKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    UpClb(clbcli,key);
end;

procedure TESvParam.clbaccKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    UpClb(clbacc,key);
end;

procedure TESvParam.clbOPKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    UpClb(clbop,key);
end;

end.
