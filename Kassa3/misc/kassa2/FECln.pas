unit FECln;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, RXCtrls, StdCtrls, DB, adsdata, adsfunc, adstable, adsrelate,
  Mask, ToolEdit, RxLookup;

type
  TECln = class(TForm)
    Label1: TLabel;
    eName: TEdit;
    Label2: TLabel;
    ePassport: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    Label3: TLabel;
    rleGrp: TRxLookupEdit;
    qGrp: TAdsQuery;
    dsqgrp: TDataSource;
    procedure rsbCancelClick(Sender: TObject);
    procedure rsbOKClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure eNameKeyPress(Sender: TObject; var Key: Char);
    procedure ePassportKeyPress(Sender: TObject; var Key: Char);
  private
    { Private declarations }
  public
    { Public declarations }
    id: integer;
  end;

var
  ECln: TECln;

implementation

uses Unit2;

{$R *.dfm}

procedure TECln.rsbCancelClick(Sender: TObject);
begin
    modalresult:=mrCancel;
end;

procedure TECln.rsbOKClick(Sender: TObject);
var
    er:string;
    q:tadsquery;
begin
    er:='';
    q:=adsq(datadir,'select * from "'+cli_name+'" where (c_name='+
            quotedstr(ename.text)+') and (id<>'+inttostr(id)+')');
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then
            er:=er+'Контрагент с таким именем уже существует'#13;
        q.close;
        q.free;
    end;
    if er='' then modalresult:=mrok else showmessage('Ошибки:'#13#13+er);
end;

procedure TECln.FormShow(Sender: TObject);
begin
    adsqq(qgrp,datadir,'select distinct c_group from "'+cli_name+'" order by c_group');
    if id<0 then begin
        ename.text:='';
        epassport.text:='';
    end;
    ename.setfocus;
end;

procedure TECln.eNameKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        perform(wm_nextdlgctl,0,0);
    end;
end;

procedure TECln.ePassportKeyPress(Sender: TObject; var Key: Char);
begin
    if key=#13 then begin
        key:=#0;
        rsbokclick(rsbok);
    end;
end;

end.
