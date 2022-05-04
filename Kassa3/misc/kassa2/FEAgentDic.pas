unit FEAgentDic;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, DBGrids, RXDBCtrl, StdCtrls, RXCtrls, DB, adsdata,
  adsfunc, adstable, adsrelate;

type
  TEAgentDic = class(TForm)
    Label1: TLabel;
    dbgA: TRxDBGrid;
    Label2: TLabel;
    eAgent: TEdit;
    Label3: TLabel;
    ePassport: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    qa: TAdsQuery;
    dsqa: TDataSource;
    Label4: TLabel;
    procedure FormShow(Sender: TObject);
    procedure rsbOKClick(Sender: TObject);
    procedure rsbCancelClick(Sender: TObject);
    procedure dbgACellClick(Column: TColumn);
    procedure dbgADblClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    id: integer;
  end;

var
  EAgentDic: TEAgentDic;

implementation

uses Unit2;

{$R *.dfm}

procedure TEAgentDic.FormShow(Sender: TObject);
begin
    adsqq(qa,datadir,'select * from "'+cli_name+'" order by c_name');
    dbga.visible:=(qa.Active) and (qa.recordcount>0);
    id:=-1;
end;

procedure TEAgentDic.rsbOKClick(Sender: TObject);
begin
    modalresult:=mrok;
end;

procedure TEAgentDic.rsbCancelClick(Sender: TObject);
begin
    modalresult:=mrcancel;
end;

procedure TEAgentDic.dbgACellClick(Column: TColumn);
begin
    if not ((qa.active) and (qa.recordcount>0)) then exit;
    eagent.Text:=qa.fieldbyname('c_name').asstring;
    epassport.text:=qa.fieldbyname('c_passport').asstring;
    id:=qa.fieldbyname('id').asinteger;
end;

procedure TEAgentDic.dbgADblClick(Sender: TObject);
begin
    dbgacellclick(nil);
    modalresult:=mrok;
end;

end.
