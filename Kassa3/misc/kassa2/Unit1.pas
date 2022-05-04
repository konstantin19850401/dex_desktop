unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ActnList, Menus, ComCtrls, ToolWin, ImgList, Placemnt, inifiles, adsrelate,
  adstable, AppEvnts, dlgrelate, ExtCtrls, RXCtrls, Grids, DBGridEh, DB,
  RxMemDS, StdCtrls, Mask, ToolEdit, PrnDbgeh, sysrelate, versioninfo,
  profunctions;

const
    BalanceHeight: array[false..true] of integer=(16,111);

type
  TForm1 = class(TForm)
    sb: TStatusBar;
    MainMenu1: TMainMenu;
    al1: TActionList;
    aUSDCourse: TAction;
    aSetup: TAction;
    aAccounts: TAction;
    aBookmarks: TAction;
    aSvodka: TAction;
    aPrihod: TAction;
    aRashod: TAction;
    N1: TMenuItem;
    N2: TMenuItem;
    N3: TMenuItem;
    aExit: TAction;
    N4: TMenuItem;
    N5: TMenuItem;
    N6: TMenuItem;
    N7: TMenuItem;
    N8: TMenuItem;
    N9: TMenuItem;
    N10: TMenuItem;
    N11: TMenuItem;
    N12: TMenuItem;
    ToolBar1: TToolBar;
    il: TImageList;
    ToolButton1: TToolButton;
    ToolButton2: TToolButton;
    ToolButton3: TToolButton;
    ToolButton4: TToolButton;
    ToolButton5: TToolButton;
    ToolButton6: TToolButton;
    ToolButton7: TToolButton;
    ToolButton8: TToolButton;
    ToolButton9: TToolButton;
    ToolButton10: TToolButton;
    ToolButton11: TToolButton;
    fs: TFormStorage;
    pmacc: TPopupMenu;
    ae1: TApplicationEvents;
    N13: TMenuItem;
    N14: TMenuItem;
    aOrders: TAction;
    ToolButton12: TToolButton;
    pmsv: TPopupMenu;
    BalancePanel: TPanel;
    Panel1: TPanel;
    rsbBalanceSwitch: TRxSpeedButton;
    Panel2: TPanel;
    mdBal: TRxMemoryData;
    dsmdBal: TDataSource;
    dbgBal: TDBGridEh;
    mdBalID: TIntegerField;
    mdBalNal: TCurrencyField;
    mdBalBN: TCurrencyField;
    mdBalUSD: TCurrencyField;
    Label1: TLabel;
    deBalDate: TDateEdit;
    rsbBalCalc: TRxSpeedButton;
    RxSpeedButton2: TRxSpeedButton;
    mdBalName: TStringField;
    pdbgBal: TPrintDBGridEh;
    il2: TImageList;
    N15: TMenuItem;
    aWindowsUp: TAction;
    ToolButton13: TToolButton;
    ToolButton14: TToolButton;
    pmWin: TPopupMenu;
    pmToolMain: TPopupMenu;
    N16: TMenuItem;
    aButtonPanel: TAction;
    Action1: TAction;
    N17: TMenuItem;
    N18: TMenuItem;
    N19: TMenuItem;
    aDataExport: TAction;
    aDataImport: TAction;
    procedure aExitExecute(Sender: TObject);
    procedure FormCloseQuery(Sender: TObject; var CanClose: Boolean);
    procedure aSetupExecute(Sender: TObject);
    procedure aUSDCourseExecute(Sender: TObject);
    procedure aPrihodExecute(Sender: TObject);
    procedure aRashodExecute(Sender: TObject);
    procedure ae1Hint(Sender: TObject);
    procedure aOrdersExecute(Sender: TObject);
    procedure aSvodkaExecute(Sender: TObject);
    procedure aAccountsExecute(Sender: TObject);
    procedure rsbBalanceSwitchClick(Sender: TObject);
    procedure Panel1Click(Sender: TObject);
    procedure rsbBalCalcClick(Sender: TObject);
    procedure dbgBalSortMarkingChanged(Sender: TObject);
    procedure RxSpeedButton2Click(Sender: TObject);
    procedure aWindowsUpExecute(Sender: TObject);
    procedure aButtonPanelExecute(Sender: TObject);
    procedure Action1Execute(Sender: TObject);
    procedure fsSavePlacement(Sender: TObject);
    procedure fsRestorePlacement(Sender: TObject);
    procedure aDataExportExecute(Sender: TObject);
  private
    procedure CommonNewOperation(aoid: integer);
    procedure AccountItemClick(Sender: TObject);
    procedure AccClickRestore(Sender: TObject);
    procedure SvClickRestore(Sender: TObject);
    procedure OrdClickRestore(Sender: TObject);
    { Private declarations }
  public
    procedure CommonWindowsRefresh(jid, aaction: integer);
    procedure RefreshAccounts(jid: integer; aaction: integer);
    procedure RefreshSvodki(jid:integer; aaction: integer);
    procedure RefreshOrders;
    procedure Do_accounts_menu;
    procedure Do_windows_menu;
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses FSetup, Unit2, FECurs, FEAct, FEAcc, FOrdList, FESvParam, FESvodka,
  FEDataExport;

{$R *.DFM}

procedure TForm1.AccClickRestore(Sender: TObject);
var
    tc: teacc;
begin
    tc:=teacc(tmenuitem(sender).Tag);
    if activemdichild<>tc then tc.BringToFront;
    if tc.WindowState=wsminimized then tc.WindowState:=wsnormal;
end;

procedure TForm1.SvClickRestore(Sender: TObject);
var
    tc: tesvodka;
begin
    tc:=tesvodka(tmenuitem(sender).Tag);
    if activemdichild<>tc then tc.BringToFront;
    if tc.WindowState=wsminimized then tc.WindowState:=wsnormal;
end;

procedure TForm1.OrdClickRestore(Sender: TObject);
var
    tc: tordlist;
begin
    tc:=tordlist(tmenuitem(sender).Tag);
    if activemdichild<>tc then tc.BringToFront;
    if tc.WindowState=wsminimized then tc.WindowState:=wsnormal;
end;

procedure TForm1.Do_windows_menu;
var
    f,g:integer;
    m:tmenuitem;
begin
    n15.Clear;
    pmWin.Items.Clear;
    g:=0;
    for f:=0 to mdichildcount-1 do begin
        if mdichildren[f] is teacc then begin
            m:=newitem('Счёт: '+teacc(mdichildren[f]).acc_title,0,false,true,
                       AccClickRestore,0,'AccItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            n15.Add(m);
            m:=newitem('Счёт: '+teacc(mdichildren[f]).acc_title,0,false,true,
                       AccClickRestore,0,'PMAccItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            pmWin.Items.Add(m);
            inc(g);
        end;
        if mdichildren[f] is tesvodka then begin
            m:=newitem('Сводка: '+tesvodka(mdichildren[f]).gv('Title','?'),0,false,true,
                       SvClickRestore,0,'SvItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            n15.Add(m);
            m:=newitem('Сводка: '+tesvodka(mdichildren[f]).gv('Title','?'),0,false,true,
                       SvClickRestore,0,'PMSvItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            pmWin.Items.Add(m);
            inc(g);
        end;
        if mdichildren[f] is tordlist then begin
            m:=newitem('Журнал ордеров',0,false,true,
                       OrdClickRestore,0,'OrdItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            n15.Add(m);
            m:=newitem('Журнал ордеров',0,false,true,
                       OrdClickRestore,0,'PMOrdItem'+inttostr(f));
            m.Tag:=integer(mdichildren[f]);
            pmWin.Items.Add(m);
            inc(g);
        end;
    end;
    n15.Visible:=g>0;
    ToolButton13.Visible:=g>0;
end;

procedure TForm1.AccountItemClick(Sender: TObject);
var
    f:integer;
begin
    for f:=0 to mdichildcount-1 do
        if (mdichildren[f] is teacc) and
           (teacc(mdichildren[f]).acc_id=(sender as tmenuitem).tag) then begin
            if activemdichild<>mdichildren[f] then mdichildren[f].BringToFront;
            if mdichildren[f].WindowState=wsminimized then
                mdichildren[f].WindowState:=wsnormal;
            Do_windows_menu;
            exit;
        end;
    eacc:=teacc.Create(application,(sender as tmenuitem).tag,
                       (sender as tmenuitem).caption,CommonWindowsRefresh);
    Do_windows_menu;
end;

procedure TForm1.Do_accounts_menu; //MU OK
var
    q:tadsquery;
    m:tmenuitem;
begin
    pmacc.Items.Clear;
    n7.Clear;
    q:=adsq(datadir, format('select * from "%s" where ID IN (%s) order by name',
        [acc_name, user.GetAccountsIN(AIN_BOTH)]));
//    q:=adsq(datadir,'select * from "'+acc_name+'" order by name');
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            q.first;
            while not q.eof do begin

                m:=newitem(q.fieldbyname('name').asstring,0,false,true,
                   AccountItemClick,0,'MMAccountItem'+
                   inttostr(q.fieldbyname('id').asinteger));
                m.tag:=q.fieldbyname('id').asinteger;
                n7.Add(m);

                m:=newitem(q.fieldbyname('name').asstring,0,false,true,
                   AccountItemClick,0,'TBAccountItem'+
                   inttostr(q.fieldbyname('id').asinteger));
                m.tag:=q.fieldbyname('id').asinteger;
                pmacc.Items.Add(m);

                q.next;
            end;
        end;
        q.close;
        q.free;
    end;
end;

procedure TForm1.RefreshAccounts(jid:integer; aaction: integer);
var
    f:integer;
    sho:tform;
begin
//todo: RefreshAccounts
    if mdichildcount>0 then begin
        sho:=showwaitmsg('Синхронизация счетов');
        for f:=0 to mdichildcount-1 do
            if mdichildren[f] is teacc then
                teacc(mdichildren[f]).Do_Refresh_Account(jid,aaction);
        sho.free;
    end;
end;

procedure TForm1.RefreshSvodki(jid:integer; aaction: integer);
begin
//todo: RefreshSvodki;
end;

procedure TForm1.RefreshOrders;
var
    f:integer;
begin
    for f:=0 to mdichildcount-1 do
        if (mdichildren[f] is TordList) then
            tordlist(mdichildren[f])._ord;
end;

procedure TForm1.CommonWindowsRefresh(jid:integer; aaction: integer);
begin
    Refreshaccounts(jid,aaction);
    Refreshsvodki(jid,aaction);
    RefreshOrders;
end;

procedure TForm1.aExitExecute(Sender: TObject);
begin
    close;
end;

procedure TForm1.FormCloseQuery(Sender: TObject; var CanClose: Boolean);
begin
    dm.shutdown;
end;

procedure TForm1.aSetupExecute(Sender: TObject); //MU OK
begin
    Setup.showmodal;
    do_accounts_menu;
    application.Title:='К2: '+setup.eBD.Text;
    form1.Caption:=format('Касса 2 - [%s]: - База: «%s», Пользователь: «%s»',
        [readversioninfo(application.exename).FileVersion, setup.eBD.Text,
        user.username]);
end;

procedure TForm1.aUSDCourseExecute(Sender: TObject); //MU OK
var
    i:tinifile;
begin
    if not user.usd then exit;
    ecurs.ceCurs.Value:=curs;
    if ecurs.showmodal=mrok then begin
        curs:=ecurs.cecurs.Value;
        i:=tinifile.create(ininame);
        i.writefloat('USD Course','Value',curs);
        i.writedate('USD Course','Date',now);
        i.free;
    end;
    sb.Panels[0].Text:='Курс: '+formatfloat('# ### ##0.00',curs)+' руб.';
end;

procedure TForm1.CommonNewOperation(aoid: integer); //MU OK
const
    da:array[0..1] of string=('true','false');
    pda:array[0..1] of string=('приход','расход');
var
    q:tadsquery;
    ijid: integer;
begin
    q:=adsq(datadir, format('select * from "%s" where optype=%s and ID in (%s)',
        [ops_name, da[aoid], user.GetOpsIN]));
//    q:=adsq(datadir,'select * from "'+ops_name+'" where optype='+da[aoid]);
    if not emp(q) then begin
        eact.jid:=-1;
        eact.oid:=aoid;
        if EAct.showmodal=mrok then begin
            adsqq(q,datadir,'select max(id) mid from "'+kas_name+'"');
            if (q.Active) and (q.RecordCount>0) then
                ijid:=q.fieldbyname('mid').asinteger else ijid:=-1;
            CommonWindowsRefresh(ijid,aact_add);
        end;
    end else showmessage('Невозможно сделать '+pda[aoid]+', т.к. нет ни одной доступной операции такого типа');
    fsq(q);
end;


procedure TForm1.aPrihodExecute(Sender: TObject);
begin
    CommonNewOperation(0);
end;

procedure TForm1.aRashodExecute(Sender: TObject);
begin
    CommonNewOperation(1);
end;

procedure TForm1.ae1Hint(Sender: TObject);
begin
    sb.Panels[1].Text:=application.Hint;
end;

procedure TForm1.aOrdersExecute(Sender: TObject);
var
    f:integer;
begin
    for f:=0 to mdichildcount-1 do
        if (mdichildren[f] is TordList) then begin
            if activemdichild<>mdichildren[f] then mdichildren[f].BringToFront;
            if mdichildren[f].WindowState=wsminimized then
                mdichildren[f].WindowState:=wsnormal;
            exit;
        end;
    OrdList:=TOrdList.Create(application);
    Do_windows_menu;
end;

procedure TForm1.aSvodkaExecute(Sender: TObject); //MU OK
var
    sl:tstringlist;
begin
    if not user.svodki then exit;
    sl:=tstringlist.create;
    if fileexists(datadir+'\svodka.cfg') then sl.LoadFromFile(datadir+'\svodka.cfg');
    if setup.cbRememberLastSv.Checked then esvparam.SvData:=sl.Text else esvparam.SvData:='';
    if esvparam.ShowModal=mrok then begin
        esvodka:=tesvodka.Create(Application,esvparam.SvData);
        sl.text:=esvparam.SvData;
        sl.SaveToFile(datadir+'\svodka.cfg');
    end;
    sl.free;
end;

procedure TForm1.aAccountsExecute(Sender: TObject);
var
    xs: tpoint;
begin
    xs:=ToolButton1.ClientToScreen(point(0,ToolButton1.Height));
    pmacc.Popup(xs.X,xs.Y);
end;

procedure TForm1.rsbBalanceSwitchClick(Sender: TObject); //MU OK
const
    aa:array[false..true] of integer=(1,0);
var
    bmp: tbitmap;
begin
    if not user.balance then exit;
    bmp:=tbitmap.Create;
    il2.GetBitmap(aa[rsbBalanceSwitch.down],bmp);
    rsbbalanceswitch.Glyph.Assign(bmp);
    bmp.Free;
    BalancePanel.height:=BalanceHeight[rsbBalanceSwitch.down];
    sb.visible:=false;
    sb.Visible:=true;
end;

procedure TForm1.Panel1Click(Sender: TObject); //MU OK
begin
    if not user.balance then exit;
    rsbBalanceSwitch.down:=not rsbBalanceSwitch.down;
    rsbBalanceSwitchClick(nil);
end;

procedure TForm1.rsbBalCalcClick(Sender: TObject); //MU OK
const
    sfn:array[0..2] of string=('Nal','BN','USD');
    sd: array[0..2] of char=('n','b','u');
var
    sho:tform;
    q:tadsquery;
    stype, k:integer;
    cur: currency;
begin
    if not user.balance then exit;
    sho:=showprogressmsg('Сбор информации',0,100);
    setprogress(sho,0);

    mdbal.DisableControls;
    mdbal.Close;
    mdbal.EmptyTable;
    mdbal.Open;

    q:=adsq(datadir, format('select * from "%s" where ID in (%s) order by name',
        [acc_name, user.GetAccountsIN(AIN_BOTH)]));
//    q:=adsq(datadir,'select * from "'+acc_name+'" order by name');
    if not emp(q) then begin
        q.first;
        while not q.eof do begin
            mdbal.Append;
            mdbal.fieldbyname('id').AsInteger:=q.fieldbyname('id').asinteger;
            mdbal.FieldByName('name').AsString:=q.fieldbyname('name').AsString;
            mdbal.FieldByName('Nal').ascurrency:=0;
            mdbal.FieldByName('bn').ascurrency:=0;
            mdbal.FieldByName('usd').ascurrency:=0;
            mdbal.Post;
            q.Next;
        end;
    end;
    fsq(q);

    SetWaitmsg(sho,'Калькуляция');

    q:=adsq(datadir, format('select * from "%s" where k_date<''%s''',
        [kas_name, datetostr(deBalDate.Date+1)]));

    if not emp(q) then begin
        q.first;
        k:=0;
        setprogress(sho,0,0,q.RecordCount);
        while not q.Eof do begin
//Расход
            if q.fieldbyname('SrcType').AsInteger in [0..2] then begin
                if mdbal.Locate('id',q.fieldbyname('SrcAcc').AsInteger,[]) then begin
                    stype:=q.fieldbyname('SrcType').AsInteger;
                    cur:=mdbal.fieldbyname(sfn[stype]).AsCurrency-q.fieldbyname('s'+sd[stype]).AsCurrency;
                    mdbal.Edit;
                    mdbal.fieldbyname(sfn[stype]).AsCurrency:=cur;
                    mdbal.post;
                end;
            end;
//Приход
            if q.fieldbyname('DstType').AsInteger in [0..2] then begin
                if mdbal.Locate('id',q.fieldbyname('DstAcc').AsInteger,[]) then begin
                    stype:=q.fieldbyname('DstType').AsInteger;
                    cur:=mdbal.fieldbyname(sfn[stype]).AsCurrency+q.fieldbyname('d'+sd[stype]).AsCurrency;
                    mdbal.Edit;
                    mdbal.fieldbyname(sfn[stype]).AsCurrency:=cur;
                    mdbal.post;
                end;
            end;

            q.Next;
            inc(k);
            if k mod 50 = 0 then begin
                setprogress(sho,k);
                application.processmessages;
            end;
        end;
    end;
    fsq(q);

    mdbal.EnableControls;
    sho.free;
end;

procedure TForm1.dbgBalSortMarkingChanged(Sender: TObject); //MU OK
var
    f:integer;
    sf: string;
    desc: boolean;
begin
    if not user.balance then exit;
    sf:='';
    desc:=false;
    for f:=0 to dbgbal.SortMarkedColumns.Count-1 do begin
        desc:=(dbgbal.SortMarkedColumns[f].Title.SortMarker = smUpEh);
        if sf<>'' then sf:=sf+';';
        sf:=sf+dbgbal.SortMarkedColumns[f].FieldName;
    end;
    if sf<>'' then begin
        mdbal.disablecontrols;
        mdbal.SortOnFields(sf,true,desc);
        mdbal.enablecontrols;
    end;
end;

procedure TForm1.RxSpeedButton2Click(Sender: TObject); //MU OK
begin
    if not user.balance then exit;
    pdbgbal.Title.Clear;
    pdbgbal.title.add('Общий баланс на '+textdatetostr(debaldate.date));
    pdbgbal.preview;
end;

procedure TForm1.aWindowsUpExecute(Sender: TObject);
var
    xs:tpoint;
begin
    if n15.Visible then begin
        xs:=ToolButton13.ClientToScreen(point(0,ToolButton13.Height));
        pmWin.Popup(xs.X,xs.Y);
    end;
end;

procedure TForm1.aButtonPanelExecute(Sender: TObject);
begin
    aButtonPanel.checked:=not aButtonPanel.checked;
    ToolBar1.Visible:=aButtonPanel.checked;
end;

procedure TForm1.Action1Execute(Sender: TObject); //MU OK
begin
    if not user.balance then exit;
    rsbBalanceSwitch.down:=not rsbBalanceSwitch.down;
    rsbBalanceSwitchClick(rsbBalanceSwitch);
end;

procedure TForm1.fsSavePlacement(Sender: TObject);
begin
    fs.WriteString('FormState_',encodeformparams(self));
end;

procedure TForm1.fsRestorePlacement(Sender: TObject);
begin
    DecodeFormParams(fs.ReadString('FormState_',''), self);
end;

procedure TForm1.aDataExportExecute(Sender: TObject);
begin
  edataexport.ShowModal;
end;

end.
