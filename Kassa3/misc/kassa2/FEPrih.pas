unit FEPrih;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, RXCtrls;

type
  TEPrih = class(TForm)
    Label1: TLabel;
    lDst: TLabel;
    Label3: TLabel;
    lDate: TLabel;
    Label5: TLabel;
    lSum: TLabel;
    Label7: TLabel;
    eOsn: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  EPrih: TEPrih;

implementation

{$R *.dfm}

end.
