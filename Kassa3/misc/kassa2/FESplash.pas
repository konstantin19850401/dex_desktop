unit FESplash;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, versioninfo;

type
  TSplash = class(TForm)
    Image1: TImage;
    Label1: TLabel;
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Splash: TSplash;

implementation

{$R *.dfm}

procedure TSplash.FormCreate(Sender: TObject);
begin
    label1.Caption:='Ê2: '+readversioninfo(application.exename).FileVersion;
end;

end.
