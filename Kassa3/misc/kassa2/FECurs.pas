unit FECurs;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  RXCtrls, StdCtrls, Mask, ToolEdit, CurrEdit;

type
  TECurs = class(TForm)
    Label1: TLabel;
    ceCurs: TCurrencyEdit;
    bOk: TButton;
    bCancel: TButton;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  ECurs: TECurs;

implementation

{$R *.DFM}

end.
