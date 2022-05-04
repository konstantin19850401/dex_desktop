unit FEOp;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, RXCtrls, Db, adsdata, adsfunc, adstable;

type
  TEOp = class(TForm)
    Label1: TLabel;
    ename: TEdit;
    gbBehavior: TGroupBox;
    Label2: TLabel;
    cbOpType: TComboBox;
    Bevel1: TBevel;
    Bevel2: TBevel;
    Label3: TLabel;
    Label4: TLabel;
    cbns: TCheckBox;
    cbbs: TCheckBox;
    cbus: TCheckBox;
    cbcs: TCheckBox;
    cbnd: TCheckBox;
    cbbd: TCheckBox;
    cbud: TCheckBox;
    cbcd: TCheckBox;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    Label5: TLabel;
    procedure FormShow(Sender: TObject);
    procedure cbOpTypeChange(Sender: TObject);
    procedure rsbOKClick(Sender: TObject);
    procedure rsbCancelClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    d: tadsquery;
    ch_behavior:boolean;
    id: integer;
  end;

var
  EOp: TEOp;

implementation

{$R *.DFM}

procedure TEOp.FormShow(Sender: TObject);
begin
    if id<0 then begin
        gbBehavior.visible:=true;
        ename.text:='';
        cbOpType.ItemIndex:=0;
        cbns.checked:=false;
        cbbs.checked:=false;
        cbus.checked:=false;
        cbcs.checked:=false;
        cbnd.checked:=false;
        cbbd.checked:=false;
        cbud.checked:=false;
        cbcd.checked:=false;
        cbOpTypeChange(cbOpType);        
    end else gbBehavior.visible:=ch_behavior;
    ename.setfocus;
end;

procedure TEOp.cbOpTypeChange(Sender: TObject);
begin
    cbcs.visible:=(cboptype.itemindex=0);
    if not cbcs.visible then cbcs.checked:=false;
    cbcd.visible:=(cboptype.itemindex=1);
    if not cbcd.visible then cbcd.checked:=false;
end;

procedure TEOp.rsbOKClick(Sender: TObject);
var
    er: string;
begin
    er:='';
    if (d.locate('name',ename.text,[locaseinsensitive])) and
       (d.fieldbyname('id').asinteger<>id) then
        er:=er+'Такая операция уже сущесвует'#13; 
    if (not cbns.checked) and (not cbbs.checked) and
       (not cbus.checked) and (not cbcs.checked) then
        er:=er+'Не указано ни одного источника'#13;
    if (not cbnd.checked) and (not cbbd.checked) and
       (not cbud.checked) and (not cbcd.checked) then
        er:=er+'Не указано ни одного приёмника'#13;
    if er='' then modalresult:=mrok else showmessage('Ошибки:'#13#13+er);
end;

procedure TEOp.rsbCancelClick(Sender: TObject);
begin
    modalresult:=mrcancel;
end;

end.
