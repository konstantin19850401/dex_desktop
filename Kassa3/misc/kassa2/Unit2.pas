unit Unit2;

// REC = YYYYMMDDHHNNSSXXXRRR - Timestamp (20)
// Y - Year, M - Month, D - Day, H - Hour, N - Minute, S - Second, X - Msec, R - Random
// REV - Revision (def. 0)

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  filectrl, sysrelate, adsrelate, Db, adsdata, adsfunc, adstable, inifiles,
  FR_Class, dlgrelate, versioninfo, DBGridEh, xmlrelate;

const
    yn:array[false..true] of string=('false','true');
    ops_name='operations.adt';
    bmk_name='bookmarks.adt';
    cli_name='clients.adt';
    kas_name='kassa.adt';
    acc_name='accounts.adt';
    ord_name='orders.adt';
    usr_name='users.adt';
    del_name='deleted.adt';
//    svo_name='svodki.adt';

    aact_add=1;
    aact_modify=2;
    aact_delete=3;
    pr_Color: tcolor=clWhite;
    ra_Color: tcolor=$fff0c0;

type
  Tdm = class(TDataModule)
    frPrihod: TfrReport;
    frRashod: TfrReport;
    frShortRashod: TfrReport;
    sdCsv: TSaveDialog;
  private
    function Check_today_ops_exist: boolean;
    procedure user_set_controls;
    { Private declarations }
  public
    { Public declarations }
    procedure prestartup;
    procedure Startup;
    procedure Shutdown;
    procedure check_db(ddir: string);
    procedure check_curs;
    procedure SetReportGlobals(aFR: TFrReport; d: tstringlist);
    function OrderNextNum(aAccID, SrcOpType: integer):integer;
    procedure PrintOrder(aOid,adt: integer; shortorder: boolean);
    procedure SaveDbgToCsv(dbg: TDbGridEh);
  end;

  TAccountRights=record
    id: integer;
    op_in, op_out: boolean;
  end;

  TOpRights=record
    id: integer;
    en: boolean;
  end;

  TUserRights=class
  private
    FAccounts: array of TAccountRights;
    FOps: array of TOpRights;
    function GetAccount(ix: integer): TAccountRights;
    function GetOp(ix: integer): TOpRights;
  public
    userid: integer;
    username: string;
    usd, svodki, balance, delrecords, impexp,
    dicusers, dicaccounts, dicclients, dicclientsadd, dicops: boolean;
    property Account[ix: integer]: TAccountRights read GetAccount;
    property Op[ix: integer]: TOpRights read GetOp;
    constructor create(aSrc: string);
    destructor destroy; override;
    function GetAccountsIN(aIn: integer): string;
    function GetOpsIN: string;
  end;

const
    AIN_IN = 1;
    AIN_OUT = 2;
    AIN_BOTH = 3;

var
  dm: Tdm;
  ininame,
  datadir:string;
  curs: currency;
  opstoday: boolean;
  User: TUserRights;

function DoAdsCallback(usPercent: Word): LongWord; stdcall;
procedure AutoColWidth(dbg:tdbgrideh; lind,rind:integer);

function GetTimeStamp: string;
procedure SetDeletedRecord(aUserID: integer; aTS, aTable: string);

implementation

uses FSetup, Unit1, FECurs, FEAEd, FEAct, FEDataExport;

{$R *.DFM}

{ Tdm }

function Get_field_str_value(f:tfield):string;
begin
    case f.datatype of
        ftstring: result:=f.asstring;
        ftsmallint,
        ftinteger,
        ftword,
        ftlargeint: result:=inttostr(f.asinteger);
        ftboolean: result:=inttostr(integer(f.asboolean));
        ftdate: result:=datetostr(f.AsDateTime);
        fttime: result:=timetostr(f.AsDateTime);
        ftdatetime: result:=datetostr(f.AsDateTime)+' '+timetostr(f.AsDateTime);
        ftfloat: result:=floattostr(f.AsFloat);
        ftcurrency: result:=formatfloat('# ### ### ### ##0.00',f.AsFloat);
    end;
end;

procedure AutoColWidth(dbg:tdbgrideh; lind,rind:integer);
var
    mwh:array of integer;
    a,f,cnt:integer;
    b:tbookmark;
    bf:tdataset;
begin
    if assigned(dbg) then
        if dbg is tdbgrideh then
            if assigned(dbg.datasource) then begin
                bf:=dbg.datasource.DataSet;
                if bf.active then begin
                    cnt:=dbg.Columns.Count;
                    if cnt>0 then begin
                        setlength(mwh,cnt);
                        for f:=0 to cnt-1 do
                            mwh[f]:=lind+rind;
                        if bf.recordcount>0 then begin
                            bf.DisableControls;
                            b:=bf.getbookmark;
                            bf.first;
                            dbg.canvas.Font.Size:=dbg.Font.Size;
                            dbg.canvas.Font.Name:=dbg.font.name;
                            while not bf.eof do begin
                                for f:=0 to cnt-1 do begin
                                    a:=lind+rind+dbg.Canvas.TextWidth(Get_field_str_value(dbg.columns[f].Field));
                                    if a>mwh[f] then mwh[f]:=a;
                                end;
                                bf.next;
                            end;
                            for f:=0 to cnt-1 do
                                dbg.Columns[f].width:=mwh[f];
                            if b<>nil then begin
                                bf.gotobookmark(b);
                                bf.freebookmark(b);
                            end;
                            bf.enablecontrols;
                        end;
                    end;
                end;
            end;
end;

function DoAdsCallback(usPercent: Word): LongWord; stdcall;
var
    bk:integer;
begin
    if screen.Cursor=1 then bk:=2 else bk:=1;
    screen.Cursors[bk]:=create_prc_cursor(0,200,usPercent);
    screen.cursor:=bk;
    result:=0;
end;

procedure Tdm.Shutdown;
begin
    form1.fs.saveformplacement;
    setup.fs.saveformplacement;
    eact.fs.SaveFormPlacement;
    edataexport.fs.SaveFormPlacement;
    User.Free;
end;

procedure Tdm.prestartup;
begin
//    User.userid:=-1;
    if (paramcount>0) and (trim(paramstr(1))<>'') and directoryexists(paramstr(1)) then
        datadir:=excludetrailingbackslash(paramstr(1)) else
        datadir:=includetrailingbackslash(extractfilepath(application.exename))+'data';
    if not directoryexists(datadir) then forcedirectories(datadir);
end;

procedure Check_iid(tbl: string);
var
  q: tadsquery;
  i: integer;
begin
  q := adsq(datadir, 'select id from "' + tbl + '" where i_id is null');
  if not emp(q) then
  begin
    i := AdsIMAX(datadir, tbl, 'i_id') + 1;
    if i < 0 then i := 0;
    while not q.Eof do
    begin
      adse(datadir, 'update "' + tbl + '" set i_id=' + inttostr(i) + ' where id=' +
        inttostr(q.fieldbyname('id').AsInteger));
      inc(i);
      q.Next;
    end;
  end;
  fsq(q);
end;

procedure Tdm.Startup;
begin
    ininame:=datadir+'\kassa2.cfg';
    check_db(datadir);
    form1.fs.inifilename:=ininame;
    form1.fs.IniSection:=User.username+'/Form1';
    form1.fs.restoreformplacement;
    setup.fs.inifilename:=ininame;
    setup.fs.restoreformplacement;
    eact.fs.IniFileName:=ininame;
    eact.fs.IniSection:=User.username+'/Eact';
    eact.fs.RestoreFormPlacement;

    edataexport.fs.IniFileName:=ininame;
    edataexport.fs.IniSection:=User.username+'/EDataExport';
    edataexport.fs.RestoreFormPlacement;

    form1.Do_accounts_menu;
    form1.rsbBalanceSwitchClick(nil);
    form1.debaldate.Date:=now;
    if form1.rsbBalanceSwitch.Down then form1.rsbBalCalcClick(nil);
    form1.ToolBar1.Visible:=form1.n16.checked;
    check_curs;
    application.Title:='К2: '+setup.eBD.Text;
    form1.Caption:=format('Касса 2 - [%s]: - База: «%s», Пользователь: «%s»',
        [readversioninfo(application.exename).FileVersion, setup.eBD.Text,
        user.username]);
    form1.Do_windows_menu;
    opstoday:=Check_today_ops_exist;
    user_set_controls;
end;

procedure Tdm.user_set_controls;
begin
    form1.aUSDCourse.Enabled:=user.usd;
    form1.aSvodka.Enabled:=user.svodki;
    form1.BalancePanel.Visible:=user.balance;
    setup.ts6.TabVisible:=user.dicusers;
    setup.ts1.TabVisible:=user.dicaccounts;
    setup.ts2.TabVisible:=(user.dicclients) and (not user.dicclientsadd);
    setup.ts3.TabVisible:=user.dicops;
    eAct.rsbAddSrc.Visible:=user.dicclients;
    eAct.rsbAddDst.Visible:=user.dicclients;
end;

function Tdm.Check_today_ops_exist: boolean;
var
    q: tadsquery;
begin
    q:=adsq(datadir, format('select count(op_id) cid from "%s" where k_date=''%s''',
        [kas_name, datetostr(now)]));
    result:=(not (emp(q))) and (q.fieldbyname('cid').asinteger>0);
    fsq(q);
end;

procedure TDM.check_db(ddir:string);
var
    sl:tstringlist;
begin
{$IFOPT D+} do_errorlogging:=false; {$ENDIF}
    sl:=tstringlist.create;
    sl.text:='Table-name='+ops_name+#13+
             'Fields=*ID autoinc|*Name char(240)|optype logical|'+
             'deleted logical|ns logical|bs logical|us logical|cs logical|'+
             'nd logical|bd logical|ud logical|cd logical|' +
             '*r_cr char(20)|*r_ch char(20)|*i_id integer';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+bmk_name+#13+
             'Fields=*ID autoinc|*B_Date date|*B_Title char(128)|B_Desc memo|' +
             '*r_cr char(20)|*r_ch char(20)';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+cli_name+#13+
             'Fields=*ID autoinc|*C_Name char(240)|*C_Passport char(240)|'+
             '*C_Group char(32)|' +
             '*r_cr char(20)|*r_ch char(20)|*i_id integer';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+kas_name+#13+
             'Fields=*ID autoinc|*Op_ID integer|*Prim char(240)|*K_Date Date|'+
             '*K_Sum curdouble|*K_USD curdouble|*SrcAcc integer|*DstAcc integer|'+
             '*SrcType integer|*DstType integer|*sn curdouble|*sb curdouble|'+
             '*su curdouble|*sc curdouble|*dn curdouble|*db curdouble|'+
             '*du curdouble|*dc curdouble|*bmk_id integer|*user_cr integer|'+
             '*user_ch integer|' +
             '*r_cr char(20)|*r_ch char(20)';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+acc_name+#13+
             'Fields=*ID autoinc|*Name char(240)|Data memo|' +
             '*r_cr char(20)|*r_ch char(20)|*i_id integer';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+ord_name+#13+
             'Fields=*ID autoinc|*JID integer|*AccID integer|*DocNum integer|'+
             '*DocDate date|*DocType integer|*Src char(128)|*Dst char(128)|'+
             '*Subj char(128)|Docsum curdouble|docusd logical|Agent char(240)|'+
             'Passport char(240)|' +
             '*r_cr char(20)|*r_ch char(20)';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+usr_name+#13+
             'Fields=*ID autoinc|*username char(64)|*password char(64)|'+
             'active logical|userlist logical|permits memo|' +
             '*r_cr char(20)|*r_ch char(20)|*i_id integer';

    ProcessTableFromDef(sl,ddir);
    sl.text:='Table-name='+del_name+#13+
             'Fields=*ID autoinc|*user_id integer|*r_cr char(20)|*tbl char(16)';
             // R_CR - timestamp удалённой записи

    ProcessTableFromDef(sl,ddir);
    sl.free;

    Check_iid(ops_name);
    check_iid(cli_name);
    check_iid(acc_name);
    check_iid(usr_name);
{$IFOPT D+} do_errorlogging:=true; {$ENDIF}
end;

procedure TDM.check_curs; //MU OK
var
    i:tinifile;
    d:tdatetime;
begin
    i:=tinifile.create(ininame);
    d:=i.readdate('USD Course','Date',0);
    curs:=i.readfloat('USD Course','Value',1);
    if (trunc(d)<>trunc(now)) and (setup.cbForceCurs.checked) then begin
        ecurs.ceCurs.Value:=curs;
        if (not user.usd) or (ecurs.showmodal=mrok) then begin
            curs:=ecurs.cecurs.Value;
            i.writefloat('USD Course','Value',curs);
            i.writedate('USD Course','Date',now);
        end;
    end;
    i.free;
    form1.sb.Panels[0].Text:='Курс: '+formatfloat('# ### ##0.00',curs)+' руб.';
end;

procedure TDM.SetReportGlobals(aFR:TFrReport; d:tstringlist);
var
    f:integer;
    l,r:string;
begin
    with aFR.dictionary do begin
        if d.count>0 then
            for f:=0 to d.Count-1 do begin
                split(d[f],'=',l,r);
                if trim(l)<>'' then variables[trim(l)]:=quotedstr(r);
            end;
    end;
end;

function TDM.OrderNextNum(aAccID, SrcOpType: integer):integer;
var
    wh: string;
    q: tadsquery;
begin
    //Приход=1
    //Расход=0
    result:=1;
    wh:='';
    if setup.cbAccNum.ItemIndex=1 then wh:=wh+'(AccID='+inttostr(aAccID)+')';
    if (setup.cbAccNum.ItemIndex=1) and (setup.cbDocNum.ItemIndex=1) then
        wh:=wh+' AND ';
    if setup.cbDocNum.ItemIndex=1 then wh:=wh+'(DocType='+inttostr(SrcOpType)+')';
    if trim(wh)<>'' then wh:=' where '+wh;
    q:=adsq(datadir,'select max(DocNum) mdn from "'+ord_name+'"'+wh);
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then result:=q.fieldbyname('mdn').asinteger+1;
        q.close;
        q.free;
    end;
end;

procedure TDM.PrintOrder(aOid,adt: integer; shortorder: boolean);
var
    q,u:tadsquery;
    sl:tstringlist;
    rept: tfrreport;
begin
    if aoid<0 then exit;
    q:=adsq(datadir,'select * from "'+ord_name+'" where (jid='+inttostr(aoid)+') and (DocType='+inttostr(adt)+')');
    //  Расход=0
    //  Приход=1
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then begin
            sl:=tstringlist.create;
            sl.text:='';
            u:=adsq(datadir,'select Data from "'+acc_name+'" where ID='+
                    inttostr(q.fieldbyname('AccID').AsInteger));
            if (u<>nil) then begin
                if (u.active) and (u.recordcount>0) then
                    sl.text:=u.fieldbyname('data').AsString;
                u.close;
                u.free;
            end;
            setvalue(sl,'Src',q.fieldbyname('Src').AsString);
            setvalue(sl,'Dst',q.fieldbyname('Dst').AsString);
            setvalue(sl,'Osnovanie',q.fieldbyname('Subj').AsString);
            setvalue(sl,'DocDate',datetostr(q.fieldbyname('DocDate').asdatetime));
            setvalue(sl,'DocDateText',textdatetostr(q.fieldbyname('DocDate').asdatetime));
            money_ru:=not q.fieldbyname('docusd').AsBoolean;
            setvalue(sl,'DocSum',formatfloat('# ### ### ##0.00',q.fieldbyname('DocSum').AsCurrency));
            setvalue(sl,'DocSumPropis',curr2txt(q.fieldbyname('DocSum').AsCurrency));
            setvalue(sl,'Passport',q.fieldbyname('Passport').AsString);
            setvalue(sl,'DocNum',inttostr(q.fieldbyname('DocNum').asinteger));
            setvalue(sl,'OpName',User.username);
            if q.fieldbyname('DocType').asinteger=0 then begin
                if shortorder then rept:=frShortRashod else rept:=frRashod;
            end else rept:=frPrihod;
            SetReportGlobals(rept,sl);
            rept.ShowReport;
        end;
        q.Close;
        q.free;
    end;
end;

function qs(s: String): string;
var
  ret: string;
  doquotes: boolean;
  f: integer;
begin
  doquotes := false;
  ret := '';
  for f := 1 to length(s) do
  begin
    if (s[f] = ';') or (s[f] = '"') then doquotes := true;
    ret := ret + s[f];
    if s[f] = '"' then ret := ret + s[f];
  end;
  if doquotes then ret := '"' + ret + '"';
  result := ret;
end;

function adds(s, val: string): string;
begin
  result := s;
  if result <> '' then result := result + ';';
  result := result + val;
end;

procedure Tdm.SaveDbgToCsv(dbg: TDbGridEh);
var
  sl: tstringlist;
  ds: tdataset;
  s: string;
  f: integer;
  olds: char;
begin
  olds := DecimalSeparator;
  DecimalSeparator := ',';

  if (dbg <> nil) and (dbg.DataSource.DataSet <> nil) then
    ds := dbg.DataSource.DataSet
  else
    ds := nil;

  if (ds <> nil) and (ds.Active) and (ds.RecordCount > 0) then
  begin
    if sdCsv.Execute then
    begin
      sl := tstringlist.Create;
      try
        s := '';
        for f := 0 to dbg.Columns.Count - 1 do
          s := adds(s, qs(dbg.Columns[f].Title.Caption));
        sl.Add(s);

        ds.First;
        while not ds.Eof do
        begin
          s := '';
          for f := 0 to dbg.Columns.Count - 1 do
            s := adds(s, qs(ds.fieldbyname(dbg.Columns[f].FieldName).AsString));
          sl.Add(s);
          ds.Next;
        end;

        sl.SaveToFile(sdCsv.FileName);
      except
      end;
      sl.Free;
    end;
  end;

  DecimalSeparator := olds;
end;


{ TUserRights }

constructor TUserRights.create(aSrc: string);
var
    xml, rt, item: trxmlnode;
    sl: tstringlist;
    f, g: integer;
begin
    sl:=tstringlist.Create;
    sl.Text:=aSrc;
    xml:=LoadRXMLDocSL(sl);
    sl.Free;
    if (xml<>nil) and (ansisametext(xml.Title,'document')) then begin
        rt:=xml.ChildByName['common'];
        if rt<>nil then begin
            usd:=(rt.ChildByName['usd']<>nil) and (ansisametext(rt.ChildByName['usd'].Value,'true'));
            Svodki:=(rt.ChildByName['svodki']<>nil) and (ansisametext(rt.ChildByName['svodki'].Value,'true'));
            balance:=(rt.ChildByName['balance']<>nil) and (ansisametext(rt.ChildByName['balance'].Value,'true'));
            delrecords:=(rt.ChildByName['delrecords']<>nil) and (ansisametext(rt.ChildByName['delrecords'].Value,'true'));
            impexp:=(rt.ChildByName['impexp']<>nil) and (ansisametext(rt.ChildByName['impexp'].Value,'true'));
            dicusers:=(rt.ChildByName['dicusers']<>nil) and (ansisametext(rt.ChildByName['dicusers'].Value,'true'));
            dicaccounts:=(rt.ChildByName['dicaccounts']<>nil) and (ansisametext(rt.ChildByName['dicaccounts'].Value,'true'));
            dicclients:=(rt.ChildByName['dicclients']<>nil) and (ansisametext(rt.ChildByName['dicclients'].Value,'true'));
            dicclientsadd:=(rt.ChildByName['dicclientsadd']<>nil) and (ansisametext(rt.ChildByName['dicclientsadd'].Value,'true'));
            dicops:=(rt.ChildByName['dicops']<>nil) and (ansisametext(rt.ChildByName['dicops'].Value,'true'));
        end;
        rt:=xml.ChildByName['accounts'];
        if rt<>nil then begin
            if rt.Count>0 then
                for f:=0 to rt.count-1 do
                    if ansisametext(rt.Children[f].Title,'item') then begin
                        item:=rt.Children[f];
                        try
                            g:=length(FAccounts);
                            setlength(FAccounts, g+1);
                            FAccounts[g].id:=strtoint(item.ChildByName['id'].Value);
                            FAccounts[g].op_in:=(item.ChildByName['in']<>nil) and
                                (ansisametext(item.ChildByName['in'].Value,'true'));
                            FAccounts[g].op_out:=(item.ChildByName['out']<>nil) and
                                (ansisametext(item.ChildByName['out'].Value,'true'));
                        except
                        end;
                    end;
        end;

        rt:=xml.ChildByName['ops'];
        if rt<>nil then begin
            if rt.Count>0 then
                for f:=0 to rt.count-1 do
                    if ansisametext(rt.Children[f].Title,'item') then begin
                        item:=rt.Children[f];
                        try
                            g:=length(FOps);
                            setlength(FOps, g+1);
                            FOps[g].id:=strtoint(item.ChildByName['id'].Value);
                            FOps[g].en:=(item.ChildByName['en']<>nil) and
                                (ansisametext(item.ChildByName['en'].Value,'true'))
                        except
                        end;
                    end;
        end;
        xml.Free;
    end;
end;

destructor TUserRights.destroy;
begin
  setlength(FAccounts, 0);
  setlength(FOps, 0);
  inherited Destroy;
end;

function TUserRights.GetAccount(ix: integer): TAccountRights;
var
    f: integer;
begin
    result.id:=ix;
    result.op_in:=false;
    result.op_out:=false;
    for f:=0 to length(FAccounts)-1 do
        if FAccounts[f].id=ix then begin
            result:=FAccounts[f];
            break;
        end;
end;

function TUserRights.GetAccountsIN(aIn: integer): string;
var
    f: integer;
begin
    result:='';
    for f:=0 to length(FAccounts)-1 do
        if ((aIn=1) and (FAccounts[f].op_in)) or
        ((aIn=2) and (FAccounts[f].op_out)) or
        ((aIn=3) and ((FAccounts[f].op_in) or (FAccounts[f].op_out))) then begin
            if result<>'' then result:=result+', ';
            result:=result+inttostr(FAccounts[f].id);
        end;
end;

function TUserRights.GetOp(ix: integer): TOpRights;
var
    f: integer;
begin
    result.id:=ix;
    result.en:=false;
    for f:=0 to length(FOps)-1 do
        if FOps[f].id=ix then begin
            result:=FOps[f];
            break;
        end;
end;

function TUserRights.GetOpsIN: string;
var
    f: integer;
begin
    result:='';
    for f:=0 to length(FOps)-1 do
        if FOps[f].en then begin
            if result<>'' then result:=result+', ';
            result:=result+inttostr(FOps[f].id);
        end;
end;

function GetTimeStamp: string;
var
  dt: tdatetime;
  y, m, d, h, s: word;
begin
  dt := now;
  DecodeDate(dt, y, m, d);
  result := lz(y, 4) + lz(m, 2) + lz(d, 2);
  DecodeTime(dt, h, m, s, y);
  result := result + lz(h, 2) + lz(m, 2) + lz(s, 2) + lz(y, 3) + lz(random(999), 3);
end;

procedure SetDeletedRecord(aUserID: integer; aTS, aTable: string);
begin
  adse(datadir, format('insert into "%s" (user_id, r_cr, tbl) values (%d, %s, %s)',
    [del_name, aUserID, quotedstr(aTS), quotedstr(aTable)]));
end;

end.
