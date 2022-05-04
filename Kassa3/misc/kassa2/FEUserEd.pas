unit FEUserEd;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, RXCtrls, adsrelate, adstable, ComCtrls, ExtCtrls, DB,
  dxmdaset, Grids, DBGridEh, xmlrelate;

type
  TeUserEd = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    cbActive: TCheckBox;
    eUsername: TEdit;
    ePassword: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    GroupBox1: TGroupBox;
    pc: TPageControl;
    ts1: TTabSheet;
    ts2: TTabSheet;
    ts3: TTabSheet;
    cbUSD: TCheckBox;
    cbSvodki: TCheckBox;
    cbBalance: TCheckBox;
    Bevel1: TBevel;
    Label4: TLabel;
    cbDicAccounts: TCheckBox;
    cbDicClients: TCheckBox;
    cbDicClientsAdd: TCheckBox;
    cbDicOps: TCheckBox;
    dbg1: TDBGridEh;
    dbg2: TDBGridEh;
    md1: TdxMemData;
    md2: TdxMemData;
    ds1: TDataSource;
    ds2: TDataSource;
    md1id: TIntegerField;
    md1name: TStringField;
    md1op_in: TBooleanField;
    md1op_out: TBooleanField;
    md2id: TIntegerField;
    md2name: TStringField;
    md2en: TBooleanField;
    cbDicUsers: TCheckBox;
    cbDelRecords: TCheckBox;
    cbImpExp: TCheckBox;
    procedure FormShow(Sender: TObject);
    procedure eUsernameKeyPress(Sender: TObject; var Key: Char);
    procedure cbActiveKeyPress(Sender: TObject; var Key: Char);
    procedure rsbOKClick(Sender: TObject);
    procedure cbDicClientsClick(Sender: TObject);
  private
    procedure SetPermits(aPerm: string);
    procedure Get_permits(out oPerm: string; out oUserList: boolean);
    { Private declarations }
  public
    { Public declarations }
    do_copy: boolean;
    id: integer;
    permits: string;
  end;

var
  eUserEd: TeUserEd;

implementation

uses Unit2;

{$R *.dfm}

procedure TeUserEd.SetPermits(aPerm: string);
var
    q: tadsquery;
    xml, rt, item: trxmlnode;
    sl: tstringlist;
    f: integer;
begin
    md1.Close;
    md1.Open;
    md2.close;
    md2.Open;

    cbusd.Checked:=false;
    cbSvodki.Checked:=false;
    cbbalance.Checked:=false;
    cbDelRecords.Checked := false;
    cbimpexp.Checked := false;
    cbdicusers.Checked:=false;
    cbdicaccounts.Checked:=false;
    cbdicclients.Checked:=false;
    cbdicclientsadd.Checked:=false;
    cbdicops.Checked:=false;

    q:=adsq(datadir, format('select * from "%s" order by name', [acc_name]));
    if not emp(q) then begin
        q.First;
        while not q.eof do begin
            md1.Append;
            md1.FieldByName('id').AsInteger:=q.fieldbyname('id').AsInteger;
            md1.fieldbyname('name').AsString:=q.fieldbyname('name').AsString;
            md1.FieldByName('op_in').AsBoolean:=false;
            md1.FieldByName('op_out').AsBoolean:=false;
            md1.Post;
            q.Next;
        end;
    end;
    fsq(q);
    q:=adsq(datadir, format('select * from "%s" order by name', [ops_name]));
    if not emp(q) then begin
        q.First;
        while not q.eof do begin
            md2.Append;
            md2.FieldByName('id').AsInteger:=q.fieldbyname('id').AsInteger;
            md2.fieldbyname('name').AsString:=q.fieldbyname('name').AsString;
            md2.FieldByName('en').AsBoolean:=false;
            md2.Post;
            q.Next;
        end;
    end;
    fsq(q);
    sl:=tstringlist.Create;
    sl.Text:=aPerm;
    xml:=LoadRXMLDocSL(sl);
    sl.Free;
    if (xml<>nil) and (ansisametext(xml.Title,'document')) then begin
        rt:=xml.ChildByName['common'];
        if rt<>nil then begin
            cbusd.Checked:=(rt.ChildByName['usd']<>nil) and (ansisametext(rt.ChildByName['usd'].Value,'true'));
            cbSvodki.Checked:=(rt.ChildByName['svodki']<>nil) and (ansisametext(rt.ChildByName['svodki'].Value,'true'));
            cbbalance.Checked:=(rt.ChildByName['balance']<>nil) and (ansisametext(rt.ChildByName['balance'].Value,'true'));
            cbDelRecords.Checked:=(rt.ChildByName['delrecords']<>nil) and (ansisametext(rt.ChildByName['delrecords'].Value,'true'));
            cbimpexp.Checked:=(rt.ChildByName['impexp']<>nil) and (ansisametext(rt.ChildByName['impexp'].Value,'true'));
            cbdicusers.Checked:=(rt.ChildByName['dicusers']<>nil) and (ansisametext(rt.ChildByName['dicusers'].Value,'true'));
            cbdicaccounts.Checked:=(rt.ChildByName['dicaccounts']<>nil) and (ansisametext(rt.ChildByName['dicaccounts'].Value,'true'));
            cbdicclients.Checked:=(rt.ChildByName['dicclients']<>nil) and (ansisametext(rt.ChildByName['dicclients'].Value,'true'));
            cbdicclientsadd.Checked:=(rt.ChildByName['dicclientsadd']<>nil) and (ansisametext(rt.ChildByName['dicclientsadd'].Value,'true'));
            cbdicops.Checked:=(rt.ChildByName['dicops']<>nil) and (ansisametext(rt.ChildByName['dicops'].Value,'true'));
        end;
        rt:=xml.ChildByName['accounts'];
        if rt<>nil then begin
            if rt.Count>0 then
                for f:=0 to rt.count-1 do
                    if ansisametext(rt.Children[f].Title,'item') then begin
                        item:=rt.Children[f];
                        try
                            if md1.Locate('id', strtoint(item.ChildByName['id'].Value), []) then begin
                                md1.edit;
                                md1.FieldByName('op_in').AsBoolean:=
                                    (item.ChildByName['in']<>nil) and
                                    (ansisametext(item.ChildByName['in'].Value,'true'));
                                md1.FieldByName('op_out').AsBoolean:=
                                    (item.ChildByName['out']<>nil) and
                                    (ansisametext(item.ChildByName['out'].Value,'true'));
                                md1.Post;
                            end;
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
                            if md2.Locate('id', strtoint(item.ChildByName['id'].Value), []) then begin
                                md2.edit;
                                md2.FieldByName('en').AsBoolean:=
                                    (item.ChildByName['en']<>nil) and
                                    (ansisametext(item.ChildByName['en'].Value,'true'));
                                md2.Post;
                            end;
                        except
                        end;
                    end;
        end;
        xml.Free;
    end;
    cbDicClientsClick(cbDicClients);
end;

procedure TeUserEd.Get_permits(out oPerm: string; out oUserList: boolean);
var
    xml, g, item: trxmlnode;
    sl: tstringlist;
begin
    xml:=trxmlnode.Create('document');
    oUserList:=cbDicUsers.Checked;

    g:=xml.AddChild('common');
    if cbusd.Checked then g.AddChild('usd', 'true');
    if cbSvodki.Checked then g.AddChild('svodki', 'true');
    if cbbalance.Checked then g.AddChild('balance', 'true');
    if cbDelRecords.Checked then g.AddChild('delrecords', 'true');
    if cbImpExp.Checked then g.AddChild('impexp', 'true');
    if cbdicusers.Checked then g.AddChild('dicusers', 'true');
    if cbdicaccounts.Checked then g.AddChild('dicaccounts', 'true');
    if cbdicclients.Checked then g.AddChild('dicclients', 'true');
    if cbdicclientsadd.Checked then g.AddChild('dicclientsadd', 'true');
    if cbdicops.Checked then g.AddChild('dicops', 'true');

    if md1.RecordCount>0 then begin
        if (md1.State=dsEdit) or (md1.State=dsInsert) then md1.Post;
        g:=xml.AddChild('accounts');
        md1.First;
        while not md1.Eof do begin
            if (md1.FieldByName('op_in').AsBoolean) or (md1.FieldByName('op_out').AsBoolean) then begin
                item:=g.AddChild('item');
                item.AddChild('id', inttostr(md1.fieldbyname('id').AsInteger));
                if md1.FieldByName('op_in').AsBoolean then item.AddChild('in', 'true');
                if md1.FieldByName('op_out').AsBoolean then item.AddChild('out', 'true');
            end;
            md1.Next;
        end;
    end;

    if md2.RecordCount>0 then begin
        if (md2.State=dsEdit) or (md2.State=dsInsert) then md2.Post;
        g:=xml.AddChild('ops');
        md2.First;
        while not md2.Eof do begin
            if md2.FieldByName('en').AsBoolean then begin
                item:=g.AddChild('item');
                item.AddChild('id', inttostr(md2.fieldbyname('id').AsInteger));
                if md2.FieldByName('en').AsBoolean then item.AddChild('en', 'true');
            end;
            md2.Next;
        end;
    end;

    sl:=SaveRXMLDocSL(xml);
    oPerm:='<?xml version="1.0" encoding="Windows-1251"?>'+sl.Text;
    sl.Free;
    xml.Free;
end;

procedure TeUserEd.FormShow(Sender: TObject);
var
    q: tadsquery;
    p: string;
begin
    p:='';
    eUsername.Text:='';
    ePassword.Text:='';
    cbActive.Checked:=true;
    if id>-1 then begin
        q:=adsq(datadir, format('select * from "%s" where id=%d',[usr_name,id]));
        if not emp(q) then begin
            eUsername.Text:=q.fieldbyname('username').AsString;
            ePassword.Text:=q.fieldbyname('password').AsString;
            cbActive.Checked:=q.fieldbyname('active').AsBoolean;
            p:=q.fieldbyname('permits').AsString;
        end else id:=-1;
        fsq(q);
    end;
    setpermits(p);
    if do_copy then begin
        eUserName.Text:=eUserName.Text+' (копия)';
        id:=-1;
    end;
    eUsername.SetFocus;
end;

procedure TeUserEd.eUsernameKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        perform(wm_nextdlgctl,0,0);
    end;
end;

procedure TeUserEd.cbActiveKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        rsbOKClick(rsbOK);
    end;
end;

procedure TeUserEd.rsbOKClick(Sender: TObject);
const
    ft: array[false..true] of string=('false','true');
var
    rts, er: string;
    ul: boolean;
    q: tadsquery;
    ts: string;
begin
    er:='';
    get_permits(rts, ul);
    if trim(eUsername.Text)='' then er:=er+'* Имя пользователя не указано'#13;
    if er='' then begin
        q:=adsq(datadir,format('select * from "%s" where (id<>%d) and (username=''%s'')',
            [usr_name,id,eUsername.Text]));
        if not emp(q) then er:=er+'* Пользователь с таким именем уже существует'#13;
        fsq(q);
    end;
    if ((not ul) or (not cbActive.Checked)) and (id>-1) then begin
        q:=adsq(datadir, format('select count(id) cid from "%s" where id<>%d and '+
            'userlist=true and active=true', [usr_name, id]));
        if (emp(q)) or (q.FieldByName('cid').AsInteger=0) then
            er:=er+'* Должен остаться хотя бы один пользователь имеющий права на изменение справочника пользователей'#13;
        fsq(q);
    end;

    if er='' then begin
        ts := GetTimeStamp;
        if id<0 then begin
            adse(datadir,format('insert into "%s" (username, password, active, '+
                'userlist, r_cr, r_ch) '+
                'values (%s,%s,%s,%s,%s,%s)',[usr_name, quotedstr(eUsername.Text),
                quotedstr(ePassword.Text),ft[cbActive.Checked], ft[ul],
                quotedstr(ts), quotedstr(ts)]));
            id:=adsimax(datadir,usr_name,'id');
        end else begin
            adse(datadir,format('update "%s" set username=%s, password=%s, '+
                'active=%s, userlist=%s, r_ch=%s where id=%d',[usr_name,
                quotedstr(eUsername.Text), quotedstr(ePassword.Text),
                ft[cbActive.Checked], ft[ul], quotedstr(ts), id]));
        end;
        SetBlobStr(datadir, usr_name, 'id', 'permits', id, rts);

        if id=User.userid then showmessage('Внимание!'#13#13+
            'Вы изменяете данные текущей учётной записи.'#13+
            'Ваши действия могут повлечь ограничения доступа с неё.'#13#13+
            'Примечание: изменения собственной учётной записи вступают в силу'#13+
            'после перезапуска программы.');
        modalresult:=mrok;
    end else showmessage('Ошибки:'#13#13+er);
end;

procedure TeUserEd.cbDicClientsClick(Sender: TObject);
begin
    cbDicClientsAdd.Enabled:=cbDicClients.Checked;
end;

end.
