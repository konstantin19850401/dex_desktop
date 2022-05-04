unit FOrdList;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ComCtrls, ExtCtrls, Grids, DBGrids, RXDBCtrl, Menus, RXCtrls,
  DB, adsdata, adsfunc, adstable, Placemnt, adsrelate, ImgList, sysrelate,
  dlgrelate, StdCtrls;

type
  TOrdList = class(TForm)
    Panel1: TPanel;
    Panel2: TPanel;
    StatusBar1: TStatusBar;
    dbgOrd: TRxDBGrid;
    rsbPrintView: TRxSpeedButton;
    rsbDeleteDoc: TRxSpeedButton;
    pmOrdType: TPopupMenu;
    N1: TMenuItem;
    N2: TMenuItem;
    qOrd: TAdsQuery;
    dsqOrd: TDataSource;
    fs: TFormStorage;
    il: TImageList;
    cbShortRO: TCheckBox;
    procedure FormShow(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure dbgOrdTitleBtnClick(Sender: TObject; ACol: Integer;
      Field: TField);
    procedure rsbPrintViewClick(Sender: TObject);
    procedure dbgOrdDrawColumnCell(Sender: TObject; const Rect: TRect;
      DataCol: Integer; Column: TColumn; State: TGridDrawState);
    procedure dbgOrdGetCellParams(Sender: TObject; Field: TField;
      AFont: TFont; var Background: TColor; Highlight: Boolean);
    procedure rsbDeleteDocClick(Sender: TObject);
    procedure dbgOrdKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure dbgOrdGetBtnParams(Sender: TObject; Field: TField;
      AFont: TFont; var Background: TColor; var SortMarker: TSortMarker;
      IsDown: Boolean);
  private
    { Private declarations }
  public
    { Public declarations }
    _order: string;
    _odesc: boolean;
    procedure _ord;
    destructor Destroy; override;
  end;

var
  OrdList: TOrdList;

implementation

uses Unit2, Unit1;

{$R *.dfm}

procedure TOrdList._ord;
const
    dck: array[false..true] of string=('',' desc');
var
    n:integer;
begin
    if (qord.active) and (qord.Recordcount>0) then
        n:=qord.fieldbyname('id').asinteger else n:=-1;
    if trim(_order)='' then _order:='ID';
    qord.DisableControls;
    adsqq(qord,datadir,'select * from "'+ord_name+'" order by '+_order+dck[_odesc]);
    qord.locate('ID',n,[]);
    qord.enablecontrols;
    dbgord.visible:=(qord.active) and (qord.recordcount>0);
end;

procedure TOrdList.FormShow(Sender: TObject);
begin
    fs.IniFileName:=ininame;
    fs.IniSection:=User.username+'/OrdList';
    fs.RestoreFormPlacement;
    _order:='';
    _odesc:=false;
    _ord;
end;

procedure TOrdList.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    fs.SaveFormPlacement;
    action:=cafree;
end;

procedure TOrdList.dbgOrdTitleBtnClick(Sender: TObject; ACol: Integer;
  Field: TField);
begin
    if ansisametext(field.FieldName,_order) then _odesc:=not _odesc else begin
        _order:=field.FieldName;
        _odesc:=false;
    end;
    _ord;
end;

procedure TOrdList.rsbPrintViewClick(Sender: TObject);
begin
    if not ((qord.active) and (qord.recordcount>0)) then exit;
    dm.PrintOrder(qord.fieldbyname('jid').AsInteger,qord.fieldbyname('doctype').AsInteger,cbShortRO.Checked);
end;

procedure TOrdList.dbgOrdDrawColumnCell(Sender: TObject; const Rect: TRect;
  DataCol: Integer; Column: TColumn; State: TGridDrawState);
begin
    if ansisametext(column.Field.FieldName,'DocType') then begin
        dbgord.Canvas.FillRect(rect);
        il.Draw(dbgord.Canvas,rect.left+1,rect.top+1,column.field.AsInteger);
    end;
end;

procedure TOrdList.dbgOrdGetCellParams(Sender: TObject; Field: TField;
  AFont: TFont; var Background: TColor; Highlight: Boolean);
const
    clcl: array[0..1] of tcolor=($fff0c0,clWhite);
begin
    background:=clcl[field.dataset.fieldbyname('DocType').asinteger];
    if highlight then begin
        afont.color:=background;
        background:=clblack;
    end;
end;

procedure TOrdList.rsbDeleteDocClick(Sender: TObject);
const
    para:array[0..1] of string=('расходный','приходный');
begin
    if not ((qord.active) and (qord.recordcount>0)) then exit;

    if not user.delrecords then
    begin
      showmessage('Недостаточно прав для удаления записей');
      exit;
    end;
    
    if dlg('Удаление ордера','Вы уверены в том, что хотите удалить'+#13+
           para[qord.fieldbyname('DocType').asinteger]+' ордер №'+
           inttostr(qord.fieldbyname('DocNum').AsInteger)+' ('+
           datetostr(qord.fieldbyname('DocDate').AsDateTime)+') ?',
           'Да, удалить|Нет, отмена',nil)=0 then begin
        adse(datadir,'delete from "'+ord_name+'" where id='+
             inttostr(qord.fieldbyname('id').AsInteger));
        _ord;
    end;
end;

procedure TOrdList.dbgOrdKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
    if key=vk_delete then rsbDeleteDocClick(rsbDeleteDoc);
end;

procedure TOrdList.dbgOrdGetBtnParams(Sender: TObject; Field: TField;
  AFont: TFont; var Background: TColor; var SortMarker: TSortMarker;
  IsDown: Boolean);
const
    vclr: array[false..true] of tcolor=(clBtnFace,$80ffff);
    odc:array[false..true] of tsortmarker=(smDown,smUp);
begin
    if ansisametext(Field.FieldName,_order) then SortMarker:=odc[_odesc] else SortMarker:=smNone;
    Background:=vclr[IsDown];
end;

destructor TOrdList.Destroy;
begin
  inherited Destroy;
  Form1.Do_windows_menu;
end;

end.
