unit FSetup;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ExtCtrls, ComCtrls, Grids, DBGrids, RXDBCtrl, RXCtrls, Db, adsdata,
  adsfunc, adstable, Placemnt,adsrelate, dlgrelate, ImgList, sysrelate,
  StdCtrls;

const
    dsc:array[false..true] of string=('','desc');

type
  TSetup = class(TForm)
    pc1: TPageControl;
    StatusBar1: TStatusBar;
    ts1: TTabSheet;
    ts2: TTabSheet;
    ts3: TTabSheet;
    ts5: TTabSheet;
    Panel1: TPanel;
    Panel2: TPanel;
    rsbAddAcc: TRxSpeedButton;
    rsbEditAcc: TRxSpeedButton;
    rsbDeleteAcc: TRxSpeedButton;
    dbga: TRxDBGrid;
    qacc: TAdsQuery;
    dsqacc: TDataSource;
    fs: TFormStorage;
    Panel3: TPanel;
    Panel4: TPanel;
    rsbAddCln: TRxSpeedButton;
    rsbEditCln: TRxSpeedButton;
    rsbDeleteCln: TRxSpeedButton;
    dbgc: TRxDBGrid;
    qcln: TAdsQuery;
    dsqcln: TDataSource;
    Panel5: TPanel;
    Panel6: TPanel;
    rsbNewOps: TRxSpeedButton;
    rsbEditOps: TRxSpeedButton;
    rsbDeleteOps: TRxSpeedButton;
    dbgo: TRxDBGrid;
    qops: TAdsQuery;
    dsqops: TDataSource;
    il: TImageList;
    cbForceCurs: TCheckBox;
    GroupBox1: TGroupBox;
    cbSaveLastDate: TCheckBox;
    cbSaveLastSum: TCheckBox;
    cbSaveLastPrim: TCheckBox;
    GroupBox2: TGroupBox;
    cbAccNum: TComboBox;
    cbDocNum: TComboBox;
    Label1: TLabel;
    eBD: TEdit;
    cbRememberLastSv: TCheckBox;
    ts6: TTabSheet;
    Panel7: TPanel;
    dbgu: TRxDBGrid;
    qusr: TAdsQuery;
    dsqusr: TDataSource;
    rsbAddUsr: TRxSpeedButton;
    rsbEditUsr: TRxSpeedButton;
    rsbDelUsr: TRxSpeedButton;
    rsbCopyUsr: TRxSpeedButton;
    lBaseID: TLabel;
    eBaseID: TEdit;
    procedure FormShow(Sender: TObject);
    procedure rsbAddAccClick(Sender: TObject);
    procedure rsbEditAccClick(Sender: TObject);
    procedure rsbDeleteAccClick(Sender: TObject);
    procedure rsbAddClnClick(Sender: TObject);
    procedure rsbEditClnClick(Sender: TObject);
    procedure rsbDeleteClnClick(Sender: TObject);
    procedure pc1Change(Sender: TObject);
    procedure dbgoDrawColumnCell(Sender: TObject; const Rect: TRect;
      DataCol: Integer; Column: TColumn; State: TGridDrawState);
    procedure rsbNewOpsClick(Sender: TObject);
    procedure rsbEditOpsClick(Sender: TObject);
    procedure rsbDeleteOpsClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure dbgcTitleBtnClick(Sender: TObject; ACol: Integer;
      Field: TField);
    procedure dbgcGetBtnParams(Sender: TObject; Field: TField;
      AFont: TFont; var Background: TColor; var SortMarker: TSortMarker;
      IsDown: Boolean);
    procedure dbguDrawColumnCell(Sender: TObject; const Rect: TRect;
      DataCol: Integer; Column: TColumn; State: TGridDrawState);
    procedure rsbAddUsrClick(Sender: TObject);
    procedure rsbEditUsrClick(Sender: TObject);
    procedure rsbDelUsrClick(Sender: TObject);
    procedure rsbCopyUsrClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    _ocln: string;
    _dcln: boolean;
    procedure _qusr;
    procedure _qacc;
    procedure _qcln;
    procedure _qops;
  end;

var
  Setup: TSetup;

implementation

uses Unit2, FEOp, FEAEd, FECln, FEUserEd;

{$R *.DFM}

procedure TSetup._qusr;
var
    rc: boolean;
    ic: integer;
begin
    if (qusr.active) and (qusr.recordcount>0) then
        ic:=qusr.fieldbyname('id').asinteger else ic:=-1;
    adsqq(qusr,datadir,'select * from "'+usr_name+'" order by username');
    rc:=(qusr.active) and (qusr.recordcount>0);
    if (visible) and (enabled) then begin
        dbgu.visible:=rc;
        rsbEditUsr.enabled:=rc;
        rsbDelUsr.enabled:=rc;
    end;
    if ic>-1 then try qusr.locate('id',ic,[]); except end;
end;

procedure TSetup._qacc;
var
    rc: boolean;
    ic: integer;
begin
    if (qacc.active) and (qacc.recordcount>0) then
        ic:=qacc.fieldbyname('id').asinteger else ic:=-1;
    adsqq(qacc,datadir,'select * from "'+acc_name+'" order by name');
    rc:=(qacc.active) and (qacc.recordcount>0);
    if (visible) and (enabled) then begin
        dbga.visible:=rc;
        rsbEditAcc.enabled:=rc;
        rsbDeleteAcc.enabled:=rc;
    end;
    if ic>-1 then try qacc.locate('id',ic,[]); except end;
end;

procedure TSetup._qcln;
var
    rc: boolean;
    ic: integer;
begin
    if (qcln.active) and (qcln.recordcount>0) then
        ic:=qcln.fieldbyname('id').asinteger else ic:=-1;
    if _ocln='' then _ocln:='c_name';
    adsqq(qcln,datadir,'select * from "'+cli_name+'" order by '+_ocln+' '+dsc[_dcln]);
    rc:=(qcln.active) and (qcln.recordcount>0);
    if (visible) and (enabled) then begin
        dbgc.visible:=rc;
        rsbEditCln.enabled:=rc;
        rsbDeleteCln.enabled:=rc;
    end;
    if ic>-1 then try qCln.locate('id',ic,[]); except end;
end;

procedure TSetup._qops;
var
    rc: boolean;
    ic: integer;
begin
    if (qops.active) and (qops.recordcount>0) then
        ic:=qops.fieldbyname('id').asinteger else ic:=-1;
    adsqq(qops,datadir,'select * from "'+ops_name+'" order by optype, name');
    rc:=(qops.active) and (qops.recordcount>0);
    if (visible) and (enabled) then begin
        dbgo.visible:=rc;
        rsbEditOps.enabled:=rc;
        rsbDeleteOps.enabled:=rc;
    end;
    if ic>-1 then try qops.locate('id',ic,[]); except end;
end;

procedure TSetup.FormShow(Sender: TObject);
begin
    _qacc;
    _qcln;
    _qops;
    _qusr;
end;

procedure TSetup.rsbAddAccClick(Sender: TObject);
begin
    eaed.id:=-1;
    if eaed.showmodal=mrok then _qacc;
end;

procedure TSetup.rsbEditAccClick(Sender: TObject);
begin
    if (not qacc.active) or (qacc.recordcount<1) then exit;
    eaed.id:=qacc.fieldbyname('id').asinteger;
    if eaed.showmodal=mrok then _qacc;
end;

procedure TSetup.rsbDeleteAccClick(Sender: TObject);
var
    q:tadsquery;
begin
    if (not qacc.active) or (qacc.recordcount<1) then exit;
    q:=adsq(datadir,'select id from "'+kas_name+
            '" where (((srctype=0) or (srctype=1) or (srctype=2)) and (srcacc='+
            inttostr(qacc.fieldbyname('id').asinteger)+')) or '+
            '(((dsttype=0) or (dsttype=1) or (dsttype=2)) and (dstacc='+
            inttostr(qacc.fieldbyname('id').asinteger)+'))');
    if ((q<>nil) and (q.active) and (q.recordcount>0)) then
        showmessage('Невозможно удалить счёт, т.к. он участвует в операциях')
    else
        if dlg('Удаление счёта','Удалить данные счёта «'+
               qacc.fieldbyname('name').asstring+'»?',
               'Да|Нет',nil)=0 then
        begin
// SyncMod 29.04.2008
            SetDeletedRecord(User.userid, qacc.fieldbyname('r_cr').AsString,
              acc_name);
// SyncMod /

            adse(datadir,'delete from "'+acc_name+'" where id='+
                 inttostr(qacc.fieldbyname('id').asinteger));
            _qacc;
        end;
    if (q<>nil) then begin
        q.close;
        q.adsclosesqlstatement;
        q.free;
    end;
end;

procedure TSetup.rsbAddClnClick(Sender: TObject);
// SyncMod 29.04.2008
var
  ts: string;
// SyncMod /
begin
    ecln.id:=-1;
    if ecln.showmodal=mrok then begin
// SyncMod 29.04.2008
        ts := GetTimeStamp;
        adse(datadir, format('insert into "%s" (c_name, c_group, c_passport, ' +
          'r_cr, r_ch) values (%s, %s, %s, %s, %s)',
          [cli_name, quotedstr(ecln.eName.Text), quotedstr(ecln.rleGrp.Text),
          quotedstr(ecln.ePassport.Text), quotedstr(ts), quotedstr(ts)]));
// SyncMod /
{
        adse(datadir,'insert into "'+cli_name+'" (c_name, c_group, c_passport) values ('+
             quotedstr(ecln.eName.Text)+', '+quotedstr(ecln.rleGrp.Text)+', '+
             quotedstr(ecln.ePassport.Text)+')');
}             
        _qcln;
    end;
end;

procedure TSetup.rsbEditClnClick(Sender: TObject);
begin
    if (not qcln.active) or (qcln.recordcount<1) then exit;
    ecln.id:=qcln.fieldbyname('id').asinteger;
    ecln.eName.Text:=qcln.fieldbyname('c_name').asstring;
    ecln.ePassport.Text:=qcln.fieldbyname('c_passport').asstring;
    ecln.rleGrp.text:=qcln.fieldbyname('c_group').AsString;
    if ecln.showmodal=mrok then begin
// SyncMod 29.04.2008
        adse(datadir, format('update "%s" set c_name=%s, c_group=%s, ' +
          'c_passport=%s, r_ch=%s where id=%d',
          [cli_name, quotedstr(ecln.ename.text), quotedstr(ecln.rleGrp.text),
          quotedstr(ecln.epassport.text), quotedstr(GetTimeStamp), ecln.id]));
// SyncMod /
{
        adse(datadir,'update "'+cli_name+'" set c_name='+quotedstr(ecln.ename.text)+
             ', c_group='+quotedstr(ecln.rleGrp.text)+
             ', c_passport='+quotedstr(ecln.epassport.text)+' where id='+inttostr(ecln.id));
}             
        _qcln;
    end;
end;

procedure TSetup.rsbDeleteClnClick(Sender: TObject);
var
    q:tadsquery;
begin
    if (not qcln.active) or (qcln.recordcount<1) then exit;
    q:=adsq(datadir,'select id from "'+kas_name+
            '" where ((srctype=3) and (srcacc='+
            inttostr(qcln.fieldbyname('id').asinteger)+')) or '+
            '((dsttype=3) and (dstacc='+
            inttostr(qcln.fieldbyname('id').asinteger)+'))');
    if ((q<>nil) and (q.active) and (q.recordcount>0)) then
        showmessage('Невозможно удалить контрагента, т.к. он участвует в операциях')
    else
        if dlg('Удаление контрагента','Удалить данные контрагента «'+
               qcln.fieldbyname('c_name').asstring+'»?',
               'Да|Нет',nil)=0 then begin
// SyncMod 29.04.2008
            SetDeletedRecord(User.userid, qcln.fieldbyname('r_cr').AsString,
              cli_name);
            adse(datadir, format('delete from "%s" where id=%d',
              [cli_name, qcln.fieldbyname('id').asinteger]));
// SyncMod /
{
            adse(datadir,'delete from "'+cli_name+'" where id='+
                 inttostr(qcln.fieldbyname('id').asinteger));
}                 
            _qcln;
        end;
    if (q<>nil) then begin
        q.close;
        q.adsclosesqlstatement;
        q.free;
    end;
end;

procedure TSetup.pc1Change(Sender: TObject);
begin
    _qcln;
    _qacc;
    _qops;
end;

procedure TSetup.dbgoDrawColumnCell(Sender: TObject;
  const Rect: TRect; DataCol: Integer; Column: TColumn;
  State: TGridDrawState);
var
    iid: integer;
begin
    iid:=windex(column.field.fieldname,'ns|bs|us|cs|nd|bd|ud|cd|optype','|');
    if iid>-1 then begin
        dbgo.Canvas.FillRect(rect);
        if iid>7 then begin
            if not column.field.asboolean then iid:=9;
        end else
            if not column.field.asboolean then iid:=-1;
        if iid>-1 then il.Draw(dbgo.canvas,rect.left+1,rect.top+1,iid);
    end;
end;

procedure TSetup.rsbNewOpsClick(Sender: TObject);
// SyncMod 29.04.2008
var
  ts: string;
// SyncMod /
begin
    eop.d:=qops;
    eop.id:=-1;
    eop.ch_behavior:=true;
    if eop.showmodal=mrok then begin
// SyncMod 29.04.2008
      ts := GetTimeStamp;
      adse(datadir, format('insert into "%s" (name, optype, ns, bs, us, cs, ' +
        'nd, bd, ud, cd, r_cr, r_ch) values (%s, %s, %s, %s, %s, %s, %s, %s, ' +
        '%s, %s, %s, %s)',
        [ops_name, quotedstr(eop.ename.text), yn[eop.cboptype.itemindex = 0],
        yn[eop.cbns.checked], yn[eop.cbbs.checked], yn[eop.cbus.checked],
        yn[eop.cbcs.checked], yn[eop.cbnd.checked], yn[eop.cbbd.checked],
        yn[eop.cbud.checked], yn[eop.cbcd.checked], quotedstr(ts),
        quotedstr(ts)]));
// SyncMod
{
        adse(datadir,'insert into "'+ops_name+
             '" (name,optype,ns,bs,us,cs,nd,bd,ud,cd) values ('+
             quotedstr(eop.ename.text)+', '+
             yn[eop.cboptype.itemindex=0]+', '+
             yn[eop.cbns.checked]+', '+yn[eop.cbbs.checked]+', '+
             yn[eop.cbus.checked]+', '+yn[eop.cbcs.checked]+', '+
             yn[eop.cbnd.checked]+', '+yn[eop.cbbd.checked]+', '+
             yn[eop.cbud.checked]+', '+yn[eop.cbcd.checked]+')');
}             
        _qops;
    end;
end;

procedure TSetup.rsbEditOpsClick(Sender: TObject);
var
    q:tadsquery;
begin
    if (not qops.active) or (qops.recordcount<1) then exit;
    eop.d:=qops;
    eop.id:=qops.fieldbyname('id').asinteger;
    q:=adsq(datadir,'select id from "'+kas_name+'" where op_id='+
            inttostr(eop.id));
    eop.ch_behavior:=not((q<>nil) and (q.active) and (q.recordcount>0));
    if q<>nil then begin
        q.close;
        q.adsclosesqlstatement;
        q.free;
    end;
    eop.ename.text:=qops.fieldbyname('name').asstring;
    if qops.fieldbyname('optype').asboolean then
        eop.cbOpType.ItemIndex:=0 else eop.cbOpType.ItemIndex:=1;
    eop.cbOpTypeChange(eop.cboptype);
    eop.cbns.checked:=qops.fieldbyname('ns').asboolean;
    eop.cbbs.checked:=qops.fieldbyname('bs').asboolean;
    eop.cbus.checked:=qops.fieldbyname('us').asboolean;
    eop.cbcs.checked:=qops.fieldbyname('cs').asboolean;
    eop.cbnd.checked:=qops.fieldbyname('nd').asboolean;
    eop.cbbd.checked:=qops.fieldbyname('bd').asboolean;
    eop.cbud.checked:=qops.fieldbyname('ud').asboolean;
    eop.cbcd.checked:=qops.fieldbyname('cd').asboolean;
    if eop.ShowModal=mrok then begin
// SyncMod 29.04.2008
      adse(datadir, format('update "%s" set name=%s, optype=%s, ns=%s, ' +
        'bs=%s, us=%s, cs=%s, nd=%s, bd=%s, ud=%s, cd=%s, r_ch=%s where id=%d',
        [ops_name, quotedstr(eop.ename.text), yn[eop.cboptype.itemindex = 0],
        yn[eop.cbns.checked], yn[eop.cbbs.checked], yn[eop.cbus.checked],
        yn[eop.cbcs.checked], yn[eop.cbnd.checked], yn[eop.cbbd.checked],
        yn[eop.cbud.checked], yn[eop.cbcd.checked], quotedstr(GetTimeStamp),
        eop.id]));
// SyncMod
{
        adse(datadir,'update "'+ops_name+'" set '+
             'name='+quotedstr(eop.ename.text)+', '+
             'optype='+yn[eop.cboptype.itemindex=0]+', '+
             'ns='+yn[eop.cbns.checked]+', '+
             'bs='+yn[eop.cbbs.checked]+', '+
             'us='+yn[eop.cbus.checked]+', '+
             'cs='+yn[eop.cbcs.checked]+', '+
             'nd='+yn[eop.cbnd.checked]+', '+
             'bd='+yn[eop.cbbd.checked]+', '+
             'ud='+yn[eop.cbud.checked]+', '+
             'cd='+yn[eop.cbcd.checked]+' where id='+inttostr(eop.id));
}             
        _qops;
    end;
end;

procedure TSetup.rsbDeleteOpsClick(Sender: TObject);
var
    q:tadsquery;
    dokill:boolean;
begin
    if (not qops.active) or (qops.recordcount<1) then exit;
    q:=adsq(datadir,'select id from "'+kas_name+'" where op_id='+
            inttostr(qops.fieldbyname('id').asinteger));
    dokill:=not((q<>nil) and (q.active) and (q.recordcount>0));
    if q<>nil then begin
        q.close;
        q.adsclosesqlstatement;
        q.free;
    end;
    if not dokill then showmessage('Невозможно удалить тип операции, т.к. он применён в текущей базе')
    else
        if dlg('Удалить тип операции','Удалить операцию «'+
               qops.fieldbyname('name').asstring+'»?',
               'Да|Нет',nil)=0 then begin
// SyncMod 29.04.2008
            SetDeletedRecord(User.userid, qops.fieldbyname('r_cr').AsString,
              ops_name);
            adse(datadir, format('delete from "%s" where id=%d',
              [ops_name, qops.fieldbyname('id').asinteger]));
// SyncMod /
{
            adse(datadir,'delete from "'+ops_name+'" where id='+
                 inttostr(qops.fieldbyname('id').asinteger));
}
            _qops;
        end;
end;

procedure TSetup.FormCreate(Sender: TObject);
begin
    _ocln:=''; _dcln:=false;
    cbAccNum.ItemIndex:=0;
    cbDocNum.ItemIndex:=0;
end;

procedure TSetup.dbgcTitleBtnClick(Sender: TObject; ACol: Integer;
  Field: TField);
begin
    if ansisametext(field.fieldname,_ocln) then _dcln:=not _dcln else begin
        _dcln:=false;
        _ocln:=field.fieldname;
    end;
    _qcln;
end;

procedure TSetup.dbgcGetBtnParams(Sender: TObject; Field: TField;
  AFont: TFont; var Background: TColor; var SortMarker: TSortMarker;
  IsDown: Boolean);
begin
    if not ansisametext(field.FieldName,_ocln) then
        sortmarker:=smnone else
        case _dcln of
            false: SortMarker:=smDown;
            true: SortMarker:=smUp;
        end;
end;

procedure TSetup.dbguDrawColumnCell(Sender: TObject; const Rect: TRect;
  DataCol: Integer; Column: TColumn; State: TGridDrawState);
const
    st: array[0..1] of string=('Пользователь','Администратор');
    ac: array[false..true] of string=('Заблокирован','Доступ разрешён');
var
    s: string;
begin
    s:='';
    if ansisametext(column.field.FieldName,'active') then s:=ac[column.field.asboolean]
    else if ansisametext(column.field.FieldName,'status') then s:=st[column.field.asinteger];
    if s<>'' then dbgu.Canvas.TextRect(rect,rect.left+2, rect.top+2,s);
end;

procedure TSetup.rsbAddUsrClick(Sender: TObject);
begin
    eUserEd.do_copy:=false;
    eUserEd.id:=-1;
    if eUserEd.showmodal=mrok then _qusr;
end;

procedure TSetup.rsbEditUsrClick(Sender: TObject);
begin
    if emp(qusr) then exit;
    eUserEd.do_copy:=false;
    eUserEd.id:=qusr.fieldbyname('id').AsInteger;
    if eUserEd.ShowModal=mrok then _qusr;
end;

procedure TSetup.rsbDelUsrClick(Sender: TObject);
var
    q: tadsquery;
    er: string;
    i: integer;
begin
    if emp(qusr) then exit;
    er:='';
    i:=qusr.fieldbyname('id').AsInteger;
    q:=adsq(datadir,format('select * from "%s" where (id<>%d) and '+
        '(userlist=true) and (active=true)',[usr_name,i]));
    if emp(q) then
        er:=er+'* Единственная учётная запись с доступом к справочнику пользователей'#13;
    fsq(q);
    q:=adsq(datadir,format('select count(id) ctid from "%s" where (user_cr=%d) or '+
        '(user_ch=%d)',[kas_name,i,i]));
    if (not emp(q)) and (q.fieldbyname('ctid').asinteger>0) then
        er:=er+'* Владелец данной записи вносил или изменял данные кассового журнала'#13;
    fsq(q);
    if er='' then begin
        if dlg('Подтверждение',format('Удалить пользователя «%s»?',
            [qusr.fieldbyname('username').asstring]),'Да|Нет',nil)=0 then begin
// SyncMod 29.04.2008
            SetDeletedRecord(User.userid, qusr.fieldbyname('r_cr').AsString,
              usr_name);
// SyncMod /
            adse(datadir,format('delete from "%s" where id=%d',[usr_name,
                qusr.fieldbyname('id').AsInteger]));

            _qusr;
        end;
    end else showmessage('Невозможно удалить пользователя по следующим причинам:'#13#13+er+#13+
        'Примечание: Вы можете изменить статус или права пользователя, '#13+
        'для ограничения доступа к базе.');
end;

procedure TSetup.rsbCopyUsrClick(Sender: TObject);
begin
    if emp(qusr) then exit;
    eUserEd.do_copy:=true;
    eUserEd.id:=qusr.fieldbyname('id').AsInteger;
    if eUserEd.ShowModal=mrok then _qusr;
end;

end.
