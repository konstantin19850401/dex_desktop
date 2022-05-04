unit FEAct;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, Mask, ToolEdit, RxLookup, CurrEdit, Db, adsdata, adsfunc,
  adstable, adsrelate, DBCtrls, sysrelate, Placemnt, RXCtrls;

type
  TEAct = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    deK_Date: TDateEdit;
    cbOpType: TComboBox;
    gbsrc: TGroupBox;
    gbdst: TGroupBox;
    cbIsrc: TComboBox;
    Label4: TLabel;
    rleNSrc: TRxLookupEdit;
    Label5: TLabel;
    Label6: TLabel;
    cbIdst: TComboBox;
    Label7: TLabel;
    rleNDst: TRxLookupEdit;
    Label8: TLabel;
    Label9: TLabel;
    Label10: TLabel;
    ceSum: TCurrencyEdit;
    ceCurs: TCurrencyEdit;
    ePrim: TEdit;
    bOk: TButton;
    bCancel: TButton;
    qop: TAdsQuery;
    dsqop: TDataSource;
    dbcbOp: TRxLookupEdit;
    qsrc: TAdsQuery;
    qdst: TAdsQuery;
    dsqsrc: TDataSource;
    dsqdst: TDataSource;
    fs: TFormStorage;
    Label11: TLabel;
    Label12: TLabel;
    lUser_cr: TLabel;
    lUser_ch: TLabel;
    rsbAddSrc: TRxSpeedButton;
    rsbAddDst: TRxSpeedButton;
    procedure cbOpTypeChange(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure bOkClick(Sender: TObject);
    procedure cbIsrcChange(Sender: TObject);
    procedure cbIdstChange(Sender: TObject);
    procedure dbcbOpChange(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure deK_DateKeyPress(Sender: TObject; var Key: Char);
    procedure ePrimKeyPress(Sender: TObject; var Key: Char);
    procedure rsbAddSrcClick(Sender: TObject);
    procedure rsbAddDstClick(Sender: TObject);
    procedure rleNSrcKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure rleNDstKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
  private
    procedure Refresh_clients(opix: integer; aads: tadsquery; const aAIN: integer = 0);
    procedure Check_curs;
    function retval(inusd, outusd: integer; sm: currency): currency;
    { Private declarations }
  public
    { Public declarations }
    mOpType, mOp, mIsrc, mNSrc, mIDst, mNDst: integer;
    oid,jid: integer;
    procedure prepare_ops_mask(acb: tcombobox; an, ab, au, ac: boolean);
  end;

var
  EAct: TEAct;

implementation

uses Unit2, FSetup, FECln;

{$R *.DFM}

procedure TEAct.prepare_ops_mask(acb:tcombobox; an, ab, au, ac: boolean);

procedure Subpop(atx:string; aix:integer; aab: boolean);
var
    i:integer;
begin
    if aab then begin
        i:=acb.items.add(atx);
        acb.items.objects[i]:=pointer(aix);
    end;
end;

begin
    acb.items.clear;
    subpop('Наличные',0,an);
    subpop('Безналичные',1,ab);
    subpop('Валюта',2,au);
    subpop('Контрагент',3,ac);
end;

procedure TEAct.FormShow(Sender: TObject); //MU OK

procedure cb2ix(acb:tcombobox; aix: integer);
var
    f:integer;
begin
    if acb.Items.Count<1 then exit;
    for f:=0 to acb.items.count-1 do
        if integer(acb.items.objects[f])=aix then begin
            acb.ItemIndex:=f;
            exit;
        end;
end;

var
    q,w:tadsquery;
label
    m1;
begin
m1:
    lUser_cr.caption:='';
    lUser_ch.caption:='';
    if jid>-1 then begin
        q:=adsq(datadir,'select * from "'+kas_name+'" where id='+inttostr(jid));
        if (q<>nil) then begin
            if emp(q) then begin
                fsq(q);
                jid:=-1;
                goto m1;
            end;
//Пользователь, вводивший запись.
            if q.fieldbyname('user_cr').asinteger>-1 then begin
                w:=adsq(datadir,format('select * from "%s" where id=%d',
                    [usr_name,q.fieldbyname('user_cr').AsInteger]));
                if not emp(w) then lUser_cr.caption:=w.fieldbyname('username').AsString;
                fsq(w);
            end;
//Пользователь, изменявший запись.
            if q.fieldbyname('user_ch').asinteger>-1 then begin
                w:=adsq(datadir,format('select * from "%s" where id=%d',
                    [usr_name,q.fieldbyname('user_ch').AsInteger]));
                if not emp(w) then lUser_ch.caption:=w.fieldbyname('username').AsString;
                fsq(w);
            end;
//Тип операции
            cboptype.itemindex:=0;
            w:=adsq(datadir,'select * from "'+ops_name+'" where id='+
                    inttostr(q.fieldbyname('op_id').asinteger));
            if not emp(w) then
                if not w.fieldbyname('optype').asboolean then cboptype.itemindex:=1;
            fsq(w);

            cbOpTypeChange(cbOpType);
//Выбор операции
            qop.Locate('id',q.fieldbyname('op_id').asinteger,[]);
            dbcbop.Text:=qop.fieldbyname('name').asstring;
//Подготовка источника
            if gbsrc.visible then begin
                cb2ix(cbisrc,q.fieldbyname('srctype').asinteger);
                cbisrcchange(cbisrc);
                qsrc.locate('id',q.fieldbyname('srcacc').asinteger,[]);
                rlensrc.text:=qsrc.fieldbyname('rec_name').asstring;
            end;
//Подготовка приёмника
            if gbdst.visible then begin
                cb2ix(cbidst,q.fieldbyname('dsttype').asinteger);
                cbidstchange(cbidst);
                qdst.locate('id',q.fieldbyname('dstacc').asinteger,[]);
                rlendst.text:=qdst.fieldbyname('rec_name').asstring;
            end;
            check_curs;
//Дата, сумма, курс и примечание
            dek_date.text:='';
            dek_date.date:=q.fieldbyname('k_date').asdatetime;
            cesum.Value:=q.fieldbyname('k_sum').ascurrency;
            cecurs.value:=q.fieldbyname('k_usd').ascurrency;
            eprim.Text:=q.fieldbyname('prim').asstring;
//Закрытие квери записи журнала
            q.close;
            q.free;
        end else goto m1;
    end else begin
//        if (not setup.cbSaveLastDate.checked) or (not opstoday) then dek_date.date:=now;
        if not setup.cbSaveLastDate.checked then dek_date.date:=now;
        if not setup.cbSaveLastSum.checked then cesum.Value:=0;
        if not setup.cbSaveLastPrim.checked then eprim.text:='';
        if not (oid in [0..1]) then oid:=0;
        cecurs.value:=curs;
        if mOptype=oid then begin
            cboptype.itemindex:=mOptype;
            cbOpTypeChange(cbOpType);
//Выбор операции
            qop.Locate('ID',mOp,[]);
            dbcbop.Text:=qop.fieldbyname('name').asstring;
//Подготовка источника
            if gbsrc.visible then begin
                cb2ix(cbisrc,misrc);
                cbisrcchange(cbisrc);
                qsrc.locate('id',mnsrc,[]);
                rlensrc.text:=qsrc.fieldbyname('rec_name').asstring;
            end;
//Подготовка приёмника
            if gbdst.visible then begin
                cb2ix(cbidst,midst);
                cbidstchange(cbidst);
                qdst.locate('id',mndst,[]);
                rlendst.text:=qdst.fieldbyname('rec_name').asstring;
            end;
        end else begin
            cboptype.itemindex:=oid;
            cbOpTypeChange(cbOpType);
        end;
    end;
    dek_date.setfocus;
end;

procedure TEAct.cbOpTypeChange(Sender: TObject); //MU OK
const
    da: array[0..1] of string=('true','false');
begin
    adsqq(qop, datadir, format('select * from "%s" where optype=%s and '+
        'ID IN (%s) order by name', [ops_name, da[cbOpType.itemindex],
        user.GetOpsIN]));

//    adsqq(qop,datadir,'select * from "'+ops_name+'" where optype='+
//        da[cbOpType.itemindex]+' order by name');

    dbcbop.visible:=(qop.active) and (qop.recordcount>0);
    gbsrc.visible:=dbcbop.visible;
    gbdst.visible:=dbcbop.visible;
    label8.visible:=dbcbop.visible;
    ceSum.visible:=dbcbop.visible;
    label9.visible:=dbcbop.visible;

    if dbcbop.visible then begin
        qop.first;
        dbcbop.text:=qop.fieldbyname('name').asstring;
        dbcbopchange(dbcbop);
    end;
end;

procedure TEAct.dbcbOpChange(Sender: TObject);
begin
    prepare_ops_mask(cbIsrc,
                     qop.fieldbyname('ns').asboolean,
                     qop.fieldbyname('bs').asboolean,
                     qop.fieldbyname('us').asboolean,
                     qop.fieldbyname('cs').asboolean);
    prepare_ops_mask(cbIdst,
                     qop.fieldbyname('nd').asboolean,
                     qop.fieldbyname('bd').asboolean,
                     qop.fieldbyname('ud').asboolean,
                     qop.fieldbyname('cd').asboolean);
    cbisrc.itemindex:=0;
    cbidst.itemindex:=0;
    cbisrcchange(cbisrc);
    cbidstchange(cbidst);
end;

procedure TEAct.Refresh_clients(opix:integer; aads: tadsquery; const aAIN: integer = 0); //MU OK
var
    nmtab,nmfld: string;
    wh: string;
begin
    wh:='';
    case opix of
        0,1,2: begin
            nmfld:='name';
            nmtab:=acc_name;
            wh:=user.GetAccountsIN(aAIN);
            if wh<>'' then wh:='where ID IN ('+wh+')';
           end;
        3: begin
            nmfld:='c_name';
            nmtab:=cli_name;
        end;
        else exit;
    end;
    adsqq(aads, datadir, format('select id, %s rec_name from "%s" %s order by %s',
        [nmfld, nmtab, wh, nmfld]));
//    adsqq(aads,datadir,'select id, '+nmfld+' as rec_name from "'+nmtab+'" order by '+nmfld);
end;

procedure TEAct.Check_curs;
begin
    ceCurs.visible:=
       (((integer(cbisrc.items.objects[cbisrc.itemindex])=2) and
         (integer(cbidst.items.objects[cbidst.itemindex])<>2))
        or
        ((integer(cbisrc.items.objects[cbisrc.itemindex])<>2) and
         (integer(cbidst.items.objects[cbidst.itemindex])=2)));
    label9.visible:=cecurs.visible;
end;

procedure TEAct.cbIsrcChange(Sender: TObject); //MU OK
var
    ok:boolean;
begin
    Refresh_clients(integer(cbisrc.Items.Objects[cbisrc.itemindex]),qsrc, AIN_OUT);
    ok:=(qsrc.active) and (qsrc.recordcount>0);
    rlensrc.visible:=ok;
    if ok then begin
        qsrc.first;
        rlensrc.Text:=qsrc.fieldbyname('rec_name').asstring;
    end;
    check_curs;
end;

procedure TEAct.cbIdstChange(Sender: TObject); //MU OK
var
    ok:boolean;
begin
    Refresh_clients(integer(cbidst.Items.Objects[cbidst.itemindex]),qdst, AIN_IN);
    ok:=(qdst.active) and (qdst.recordcount>0);
    rlendst.visible:=ok;
    if ok then begin
        qdst.first;
        rlendst.Text:=qdst.fieldbyname('rec_name').asstring;
    end;
    check_curs;
end;

procedure TEAct.FormCreate(Sender: TObject);
begin
    mOpType:=-1;
    mOp:=-1;
    mIsrc:=-1;
    mNSrc:=-1;
    mIDst:=-1;
    mNDst:=-1;
    eprim.text:='';
    cesum.value:=0; cecurs.value:=0;
    dek_date.text:=''; dek_date.date:=now;
end;

procedure TEAct.deK_DateKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        perform(wm_nextdlgctl,0,0);
    end;
end;

function TEAct.retval(inusd,outusd:integer; sm:currency):currency;
// Возвращает пересчитанную в соответствии со значением (источник-валюта, приемник-валюта)
// сумму.
begin
    result:=sm;
    if (inusd=3) or (outusd=3) then exit;
    if inusd=outusd then exit;
    dm.check_curs;
    if (inusd=2) and (outusd<>2) then result:=cap(sm*curs);
    if (inusd<>2) and (outusd=2) then result:=cap(sm/curs);
end;

procedure TEAct.bOkClick(Sender: TObject);
var
    er:string;
    sn,sb,su,sc,dn,db,du,dc: currency;
// SyncMod 29.04.2008
    ts: string;
// SyncMod /
begin
    er:='';
//Проверка на ошибки ввода
    try strtodate(dek_date.text) except er:=er+'Некорректная дата'#13; end;
    if not (cboptype.itemindex in [0..1]) then er:=er+'Некорректный тип операции'#13;
    if not ansisametext(dbcbop.text,qop.fieldbyname('name').asstring) then
        er:=er+'Некорректная операция'#13;
    if cbisrc.ItemIndex<0 then er:=er+'Источник: некорректный тип ввода'#13;
    if not ansisametext(rlensrc.text,qsrc.fieldbyname('rec_name').asstring) then
        er:=er+'Источник: некорректный счёт/контрагент'#13;
    if cbidst.ItemIndex<0 then er:=er+'Приёмник: некорректный тип ввода'#13;
    if not ansisametext(rlendst.text,qdst.fieldbyname('rec_name').asstring) then
        er:=er+'Приёмник: некорректный счёт/контрагент'#13;
    if cesum.value<=0 then er:=er+'Некорректная сумма'#13;
    if (cecurs.visible) and (cecurs.value<=0) then
        er:=er+'Некорректный курс'#13;
    if (integer(cbisrc.items.objects[cbisrc.itemindex])=
        integer(cbidst.items.objects[cbidst.itemindex])) and
        (qsrc.fieldbyname('id').asinteger=qdst.fieldbyname('id').asinteger)
        then er:=er+'Источник и приёмник не могут быть одним счётом/контрагентом'#13;

//Сохранение в базе
    if er<>'' then showmessage('Ошибки:'#13#13+er) else begin
        modalresult:=mrok;
        if trunc(deK_date.Date)=trunc(now) then opstoday:=true;
        sn:=0; sb:=0; su:=0; sc:=0;
        dn:=0; db:=0; du:=0; dc:=0;
        case integer(cbisrc.items.objects[cbisrc.itemindex]) of
            0: sn:=cesum.value;
            1: sb:=cesum.value;
            2: su:=cesum.value;
            3: sc:=cesum.value;
        end;
        case integer(cbidst.items.objects[cbidst.itemindex]) of
            0: dn:=retval(integer(cbisrc.items.objects[cbisrc.itemindex]),integer(cbidst.items.objects[cbidst.itemindex]),cesum.value);
            1: db:=retval(integer(cbisrc.items.objects[cbisrc.itemindex]),integer(cbidst.items.objects[cbidst.itemindex]),cesum.value);
            2: du:=retval(integer(cbisrc.items.objects[cbisrc.itemindex]),integer(cbidst.items.objects[cbidst.itemindex]),cesum.value);
            3: dc:=retval(integer(cbisrc.items.objects[cbisrc.itemindex]),integer(cbidst.items.objects[cbidst.itemindex]),cesum.value);
        end;
// SyncMod 29.04.2008
        ts := GetTimeStamp;
// SyncMod /
        if jid<0 then
            adse(datadir, format('insert into "%s" (op_id, prim, k_date, k_sum, '+
                'k_usd, srcacc, dstacc, srctype, dsttype, sn, sb, su, sc, dn, '+
                'db, du, dc, bmk_id, user_cr, user_ch, r_cr, r_ch) values (%d, '+
                '%s, ''%s'', %s, %s, %d, %d, %d, %d, %s, %s, %s, %s, %s, %s, '+
                '%s, %s, -1, %d, -1, %s, %s)',
                [kas_name, qop.fieldbyname('id').asinteger, quotedstr(eprim.text),
                datetostr(dek_date.Date), f2s(cesum.value), f2s(cecurs.value),
                qsrc.fieldbyname('id').asinteger, qdst.fieldbyname('id').asinteger,
                integer(cbisrc.items.objects[cbisrc.itemindex]),
                integer(cbidst.items.objects[cbidst.itemindex]),
                f2s(sn), f2s(sb), f2s(su), f2s(sc), f2s(dn), f2s(db), f2s(du),
                f2s(dc), user.userid, quotedstr(ts), quotedstr(ts)])) // SyncMod 29.04.2008
        else
            adse(datadir, format('update "%s" set op_id=%d, prim=%s, '+
                'k_date=''%s'', k_sum=%s, k_usd=%s, srcacc=%d, dstacc=%d, '+
                'srctype=%d, dsttype=%d, sn=%s, sb=%s, su=%s, sc=%s, '+
                'dn=%s, db=%s, du=%s, dc=%s, user_ch=%d, r_ch=%s where id=%d',
                [kas_name, qop.fieldbyname('id').asinteger, quotedstr(eprim.text),
                datetostr(dek_date.Date), f2s(cesum.value), f2s(cecurs.value),
                qsrc.fieldbyname('id').asinteger, qdst.fieldbyname('id').asinteger,
                integer(cbisrc.items.objects[cbisrc.itemindex]),
                integer(cbidst.items.objects[cbidst.itemindex]),
                f2s(sn), f2s(sb), f2s(su), f2s(sc), f2s(dn), f2s(db), f2s(du),
                f2s(dc), User.userid, quotedstr(ts), jid])); // SyncMod 29.04.2008

        mOpType:=cbOpType.ItemIndex;
        mOp:=qop.fieldbyname('id').asinteger;
        mISrc:=integer(cbisrc.items.objects[cbisrc.itemindex]);
        mIDst:=integer(cbidst.items.objects[cbidst.itemindex]);
        mNSrc:=qsrc.fieldbyname('id').asinteger;
        mNdst:=qdst.fieldbyname('id').asinteger;
    end;
end;

procedure TEAct.ePrimKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        bokclick(bok);
    end;
end;

procedure TEAct.rsbAddSrcClick(Sender: TObject);
begin
    if not user.dicclients then exit;
    case integer(cbIsrc.Items.Objects[cbIsrc.ItemIndex]) of
        0..2: showmessage('Новые счёта заводятся в Установках программы');
        3:
        begin
            setup.rsbAddClnClick(setup.rsbAddCln);
            if eCln.ModalResult=mrok then begin
                cbisrcchange(cbisrc);
                rleNSrc.Text:=eCln.eName.Text;
            end;
        end;
    end;
end;

procedure TEAct.rsbAddDstClick(Sender: TObject);
begin
    if not user.dicclients then exit;
    case integer(cbIdst.Items.Objects[cbIdst.ItemIndex]) of
        0..2: showmessage('Новые счёта заводятся в Установках программы');
        3:
        begin
            setup.rsbAddClnClick(setup.rsbAddCln);
            if eCln.ModalResult=mrok then begin
                cbidstchange(cbidst);
                rleNDst.Text:=eCln.eName.Text;
            end;
        end;
    end;
end;

procedure TEAct.rleNSrcKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    if key=vk_insert then rsbAddSrcClick(rsbAddSrc);
end;

procedure TEAct.rleNDstKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    if key=vk_insert then rsbAddDstClick(rsbAddDst);
end;

end.
