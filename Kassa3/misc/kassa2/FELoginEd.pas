unit FELoginEd;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, DB, adsdata, adsfunc, adstable, StdCtrls, Mask, ToolEdit,
  RxLookup, Placemnt, RXCtrls, adsrelate;

type
  TeLoginEd = class(TForm)
    rleUserName: TRxLookupEdit;
    Label1: TLabel;
    dsusr: TDataSource;
    qusr: TAdsQuery;
    fs: TFormStorage;
    Label2: TLabel;
    ePassword: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    procedure FormShow(Sender: TObject);
    procedure FormHide(Sender: TObject);
    procedure fsRestorePlacement(Sender: TObject);
    procedure rsbCancelClick(Sender: TObject);
    procedure rsbOKClick(Sender: TObject);
    procedure fsSavePlacement(Sender: TObject);
    procedure rleUserNameKeyPress(Sender: TObject; var Key: Char);
    procedure ePasswordKeyPress(Sender: TObject; var Key: Char);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  eLoginEd: TeLoginEd;

implementation

uses Unit2;

{$R *.dfm}

procedure TeLoginEd.FormShow(Sender: TObject);
begin
    adsqq(qusr,datadir,format('select * from "%s" order by username',[usr_name]));
    if emp(qusr) then begin
        adse(datadir, format('insert into "%s" (username, password, active, userlist) '+
            'values (''Встроенная учётная запись'', '''', true, true)',[usr_name]));
        SetBlobStr(datadir, usr_name, 'id', 'permits', -1,
            '<?xml version="1.0" encoding="Windows-1251"?>'#13+
            '  <common>'#13+
            '    <dicusers>true</dicusers>'#13+
            '  </common>'#13+
            '</document>');

        adsqq(qusr,datadir,format('select * from "%s" order by username',[usr_name]));
    end;
    fs.IniSection:=datadir;
    fs.RestoreFormPlacement;
    rleUserName.SetFocus;
end;

procedure TeLoginEd.FormHide(Sender: TObject);
begin
    fs.SaveFormPlacement;
    qusr.Close;
    qusr.AdsCloseSQLStatement;
    adscleanupbuffers(datadir);
end;

procedure TeLoginEd.fsRestorePlacement(Sender: TObject);
var
    s: string;
begin
    s:=fs.RegIniFile.ReadString(fs.IniSection,'LastUserName','');
    rleUserName.Text:='';
    if not emp(qusr) then
        if qusr.Locate('username',s,[locaseinsensitive]) then
            rleUserName.Text:=qusr.fieldbyname('username').AsString;
    ePassword.Text:='';
end;

procedure TeLoginEd.fsSavePlacement(Sender: TObject);
begin
    fs.RegIniFile.WriteString(fs.IniSection,'LastUserName',rleUserName.Text);
end;

procedure TeLoginEd.rsbCancelClick(Sender: TObject);
begin
    modalresult:=mrcancel;
end;

procedure TeLoginEd.rsbOKClick(Sender: TObject);
begin
    if (qusr.Locate('username',rleUserName.Text,[locaseinsensitive])) and
        (ePassword.Text=qusr.fieldbyname('password').AsString) then begin
        if qusr.FieldByName('active').AsBoolean then begin
            User:=TUserRights.create(qusr.fieldbyname('permits').AsString);
            User.userid:=qusr.fieldbyname('id').AsInteger;
            User.username:=qusr.fieldbyname('username').AsString;
            modalresult:=mrok;
        end else showmessage('Данная учётная запись блокирована');
    end else showmessage('Неправильное имя или пароль.');
end;

procedure TeLoginEd.rleUserNameKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        perform(wm_nextdlgctl,0,0);
    end;
end;

procedure TeLoginEd.ePasswordKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        rsbOKClick(rsbOK);
    end;
end;

end.
