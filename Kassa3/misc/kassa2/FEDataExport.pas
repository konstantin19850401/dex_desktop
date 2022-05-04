unit FEDataExport;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ComCtrls, StdCtrls, Mask, ToolEdit, ExtCtrls, Placemnt,
  EcXMLParser, DB, adsdata, adsfunc, adstable, adsrelate, sysrelate;

type
  TEDataExport = class(TForm)
    GroupBox1: TGroupBox;
    rbDate: TRadioButton;
    rbFull: TRadioButton;
    pDate: TPanel;
    Label1: TLabel;
    Label2: TLabel;
    deStart: TDateEdit;
    deEnd: TDateEdit;
    gbStatus: TGroupBox;
    pbStatus: TProgressBar;
    Button1: TButton;
    fs: TFormStorage;
    sd: TSaveDialog;
    procedure FormShow(Sender: TObject);
    procedure rbDateClick(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    function ExportFull(xml: tecxmlparser): string;
    function ExportRange(xml: TEcXMLParser; des, dee: tdatetime): string;
    { Private declarations }
  public
    { Public declarations }
  end;

  TIntHash = class
    i: array of integer;
  private
    function GetCount: integer;
    function GetValue(ix: integer): integer;
  public
    property Count: integer read GetCount;
    property value[ix: integer]: integer read GetValue; default;
    procedure Add(value: integer);
    procedure Clear;
    constructor Create;
    destructor Destroy; override;
  end;


var
  EDataExport: TEDataExport;

implementation

uses Unit2, FSetup;

{$R *.dfm}

procedure TEDataExport.FormShow(Sender: TObject);
begin
  gbStatus.Caption := 'Статус';
  pbStatus.Min := 0;
  pbStatus.Max := 100;
  pbStatus.Step := 0;
  rbDateClick(rbDate);
end;

procedure TEDataExport.rbDateClick(Sender: TObject);
begin
  pDate.Visible := rbDate.Checked;
end;

procedure TEDataExport.Button1Click(Sender: TObject);
const
  rng: array[false..true] of string = ('date', 'full');
var
  xml: TEcXMLParser;
  xname, er: string;
begin
  er := '';
  if trim(Setup.eBaseID.Text) = '' then
    er := er + '* Не указан идентификатор базы.'#13#13 +
               '* - Зайдите в "Параметры > Установки" и на вкладке "Общие"'#13 +
               '* - укажите идентификатор БД.'#13;
  if rbDate.Checked then
  begin
    try
      strtodate(deStart.Text);
      if deStart.Date < 1 then
        er := er + '* Некорректная начальная дата'#13;
    except
        er := er + '* Некорректная начальная дата'#13;
    end;

    try
      strtodate(deEnd.Text);
      if deStart.Date < 1 then
        er := er + '* Некорректная конечная дата'#13;
    except
        er := er + '* Некорректная конечная дата'#13;
    end;

    if er = '' then
      if trunc(deStart.Date) > trunc(deEnd.Date) then
        er := er + '* Начальная дата не может быть позже конечной'#13;
  end;

  if er = '' then
  begin
    xml := TEcXMLParser.Create(nil);
    xml.Root.Name := 'Document';
    xml.Root.Params.Values['type'] := 'kassa';
    xml.root.params.values['id'] := trim(Setup.eBaseID.Text);
    xml.Root.Params.Values['range'] := rng[rbFull.checked];
    if rbDate.Checked then
    begin
      xml.Root.Params.Values['start'] := datetostr(deStart.Date);
      xml.Root.Params.Values['end'] := datetostr(deEnd.Date);
    end;
    xml.Root.New.Name := 'Stub';
    
    if rbFull.Checked then
      er := ExportFull(xml)
    else
      er := ExportRange(xml, deStart.Date, deEnd.Date);

    if er = '' then
    begin
      if rbFull.Checked then
        xname := 'kassa_full_' +datetostr(now) + '.xml'
      else
        xname := 'kassa_' + datetostr(deStart.Date) + '-' + datetostr(deend.Date) + '.xml';

      sd.FileName := xname;

      if sd.Execute then
      begin
        xml.SaveToFile(sd.FileName);
        showmessage('Экспорт произведен');
      end;
    end;

    xml.Free;
  end;

  if er <> '' then
    showmessage('Ошибки:'#13#13 + er);

end;


function TEDataExport.ExportFull(xml: tecxmlparser): string;
var
  q, qops, qacc, qcli, qusr: tadsquery;
  i, k: txmlitem;
  f: integer;

  dsep: char;

  ops, acc, cli, usr: TIntHash;

begin
  //Полный экспорт
  ops := TIntHash.Create;
  acc := TIntHash.Create;
  cli := TIntHash.Create;
  usr := TIntHash.Create;
  q := adsq(datadir, 'select * from "' + kas_name + '" order by id');
  qops := adsq(datadir, 'select * from "' + ops_name + '" order by id');
  qacc := adsq(datadir, 'select * from "' + acc_name + '" order by id');
  qcli := adsq(datadir, 'select * from "' + cli_name + '" order by id');
  qusr := adsq(datadir, 'select * from "' + usr_name + '" order by id');
  if not emp(q) then
  begin
    k := xml.Root.New;
    k.Name := 'Table';
    k.Params.Values['name'] := 'kassa';

    dsep := DecimalSeparator;
    DecimalSeparator := '.';
    pbStatus.Max := q.RecordCount;
    pbStatus.Position := 0;

    while not q.Eof do
    begin

      ops.Add(q.fieldbyname('op_id').AsInteger);

      if btw(q.fieldbyname('srctype').AsInteger, 0, 2) then
        acc.Add(q.fieldbyname('srcacc').AsInteger)
      else
        if q.fieldbyname('srctype').AsInteger = 3 then
          cli.Add(q.fieldbyname('srcacc').AsInteger);

      if btw(q.fieldbyname('dsttype').AsInteger, 0, 2) then
        acc.Add(q.fieldbyname('dstacc').AsInteger)
      else
        if q.fieldbyname('dsttype').AsInteger = 3 then
          cli.Add(q.fieldbyname('dstacc').AsInteger);

      usr.Add(q.fieldbyname('user_cr').AsInteger);
      usr.Add(q.fieldbyname('user_ch').AsInteger);


      i := k.New;
      i.Name := 'Record';

      qops.Locate('id', q.fieldbyname('op_id').AsInteger, []);
      i.Params.Values['op_id'] := inttostr(qops.fieldbyname('i_id').AsInteger);


      i.Params.Values['k_date'] := datetostr(q.fieldbyname('k_date').AsDateTime);
      i.Params.Values['k_sum'] := CurrToStr(q.fieldbyname('k_sum').AsCurrency);
      i.Params.Values['k_usd'] := CurrToStr(q.fieldbyname('k_usd').AsCurrency);

      if btw(q.fieldbyname('srctype').AsInteger, 0, 2) then
      begin
        qacc.Locate('id', q.fieldbyname('srcacc').AsInteger, []);
        i.Params.Values['srcacc'] := inttostr(qacc.fieldbyname('i_id').AsInteger);
      end
      else if q.fieldbyname('srctype').AsInteger = 3 then
      begin
        qcli.Locate('id', q.fieldbyname('srcacc').AsInteger, []);
        i.Params.Values['srcacc'] := inttostr(qcli.fieldbyname('i_id').AsInteger);
      end;

      if btw(q.fieldbyname('dsttype').AsInteger, 0, 2) then
      begin
        qacc.Locate('id', q.fieldbyname('dstacc').AsInteger, []);
        i.Params.Values['dstacc'] := inttostr(qacc.fieldbyname('i_id').AsInteger);
      end
      else if q.fieldbyname('dsttype').AsInteger = 3 then
      begin
        qcli.Locate('id', q.fieldbyname('dstacc').AsInteger, []);
        i.Params.Values['dstacc'] := inttostr(qcli.fieldbyname('i_id').AsInteger);
      end;

      i.Params.Values['srctype'] := inttostr(q.fieldbyname('srctype').AsInteger);
      i.Params.Values['dsttype'] := inttostr(q.fieldbyname('dsttype').AsInteger);
      i.Params.Values['sn'] := CurrToStr(q.fieldbyname('sn').AsCurrency);
      i.Params.Values['sb'] := CurrToStr(q.fieldbyname('sb').AsCurrency);
      i.Params.Values['su'] := CurrToStr(q.fieldbyname('su').AsCurrency);
      i.Params.Values['sc'] := CurrToStr(q.fieldbyname('sc').AsCurrency);
      i.Params.Values['dn'] := CurrToStr(q.fieldbyname('dn').AsCurrency);
      i.Params.Values['db'] := CurrToStr(q.fieldbyname('db').AsCurrency);
      i.Params.Values['du'] := CurrToStr(q.fieldbyname('du').AsCurrency);
      i.Params.Values['dc'] := CurrToStr(q.fieldbyname('dc').AsCurrency);
      i.Params.Values['bmk_id'] := inttostr(q.fieldbyname('bmk_id').AsInteger);

      if qusr.Locate('id', q.fieldbyname('user_cr').AsInteger, []) then
        i.Params.Values['user_cr'] := inttostr(qusr.fieldbyname('i_id').AsInteger)
      else
        i.Params.Values['user_cr'] := inttostr(-1);

      if qusr.Locate('id', q.fieldbyname('user_ch').AsInteger, []) then
        i.Params.Values['user_ch'] := inttostr(qusr.fieldbyname('i_id').AsInteger)
      else
        i.Params.Values['user_ch'] := inttostr(-1);

      i.Params.Values['r_cr'] := q.fieldbyname('r_cr').AsString;
      i.Params.Values['r_ch'] := q.fieldbyname('r_ch').AsString;
      i.Text := q.fieldbyname('prim').AsString;

      q.Next;

      pbStatus.StepIt;
      if pbStatus.Position mod 10 = 0 then
        Application.ProcessMessages;
    end;

    if (usr.GetCount > 0) and (not emp(qusr)) then
    begin
      k := xml.Root.New;
      k.Name := 'Table';
      k.Params.Values['name'] := 'users';
      for f := 0 to usr.GetCount - 1 do
      begin
        if qusr.Locate('id', usr[f], []) then
        begin
          i := k.New;
          i.Name := 'Record';
          i.Params.Values['username'] := qusr.fieldbyname('username').AsString;
          i.Params.Values['password'] := qusr.fieldbyname('password').AsString;
          i.Params.Values['active'] := yn[qusr.fieldbyname('active').asboolean];
          i.Text := qusr.fieldbyname('permits').AsString;
          i.Params.Values['userlist'] := yn[qusr.fieldbyname('userlist').asboolean];
          i.Params.Values['r_cr'] := qusr.fieldbyname('r_cr').AsString;
          i.Params.Values['r_ch'] := qusr.fieldbyname('r_ch').AsString;
          i.Params.Values['i_id'] := inttostr(qusr.fieldbyname('i_id').AsInteger);
        end;
      end;
    end;

    if (cli.GetCount > 0) and (not emp(qcli)) then
    begin
      k := xml.Root.New;
      k.Name := 'Table';
      k.Params.Values['name'] := 'clients';
      for f := 0 to cli.GetCount - 1 do
      begin
        if qcli.Locate('id', cli[f], []) then
        begin
          i := k.New;
          i.Name := 'Record';
          i.Params.Values['c_name'] := qcli.fieldbyname('c_name').AsString;
          i.Params.Values['c_passport'] := qcli.fieldbyname('c_passport').AsString;
          i.Params.Values['c_group'] := qcli.fieldbyname('c_group').AsString;
          i.Params.Values['r_cr'] := qcli.fieldbyname('r_cr').AsString;
          i.Params.Values['r_ch'] := qcli.fieldbyname('r_ch').AsString;
          i.Params.Values['i_id'] := inttostr(qcli.fieldbyname('i_id').AsInteger);
        end;
      end;
    end;

    if (acc.GetCount > 0) and (not emp(qacc)) then
    begin
      k := xml.Root.New;
      k.Name := 'Table';
      k.Params.Values['name'] := 'accounts';
      for f := 0 to acc.GetCount - 1 do
      begin
        if qacc.Locate('id', acc[f], []) then
        begin
          i := k.New;
          i.Name := 'Record';
          i.Params.Values['name'] := qacc.fieldbyname('name').AsString;
          i.Text := qacc.fieldbyname('data').asstring;
          i.Params.Values['r_cr'] := qacc.fieldbyname('r_cr').AsString;
          i.Params.Values['r_ch'] := qacc.fieldbyname('r_ch').AsString;
          i.Params.Values['i_id'] := inttostr(qacc.fieldbyname('i_id').AsInteger);
        end;
      end;
    end;

    if (ops.GetCount > 0) and (not emp(qops)) then
    begin
      k := xml.Root.New;
      k.Name := 'Table';
      k.Params.Values['name'] := 'operations';
      for f := 0 to ops.GetCount - 1 do
      begin
        if qops.Locate('id', ops[f], []) then
        begin
          i := k.New;
          i.Name := 'Record';
          i.Params.Values['name'] := qops.fieldbyname('name').AsString;
          i.Params.Values['optype'] := yn[qops.fieldbyname('optype').asboolean];
          i.Params.Values['deleted'] := yn[qops.fieldbyname('deleted').asboolean];

          i.Params.Values['ns'] := yn[qops.fieldbyname('ns').asboolean];
          i.Params.Values['bs'] := yn[qops.fieldbyname('bs').asboolean];
          i.Params.Values['us'] := yn[qops.fieldbyname('us').asboolean];
          i.Params.Values['cs'] := yn[qops.fieldbyname('cs').asboolean];
          i.Params.Values['nd'] := yn[qops.fieldbyname('nd').asboolean];
          i.Params.Values['bd'] := yn[qops.fieldbyname('bd').asboolean];
          i.Params.Values['ud'] := yn[qops.fieldbyname('ud').asboolean];
          i.Params.Values['cd'] := yn[qops.fieldbyname('cd').asboolean];

          i.Params.Values['r_cr'] := qops.fieldbyname('r_cr').AsString;
          i.Params.Values['r_ch'] := qops.fieldbyname('r_ch').AsString;
          i.Params.Values['i_id'] := inttostr(qops.fieldbyname('i_id').AsInteger);
        end;
      end;
    end;


    fsq(q);
    fsq(qusr);
    fsq(qcli);
    fsq(qacc);
    fsq(qops);
    usr.Free;
    cli.Free;
    acc.Free;
    ops.Free;
    pbStatus.Position := 0;
    DecimalSeparator := dsep;
  end;
end;

function TEdataExport.ExportRange(xml: TEcXMLParser; des, dee: tdatetime): string;
begin
  // Экспорт диапазона дат
end;








{ TIntHash }

procedure TIntHash.Add(value: integer);
var
  f, id: integer;
begin
  id := -1;
  for f := 0 to length(i) - 1 do
    if i[f] = value then
    begin
      id := f;
      break;
    end;

  if id < 0 then
  begin
    setlength(i, length(i) + 1);
    i[length(i) - 1] := value;
  end;

end;

procedure TIntHash.Clear;
begin
  setlength(i, 0);
end;

constructor TIntHash.Create;
begin
  inherited Create;
  clear;
end;

destructor TIntHash.Destroy;
begin
  clear;
  inherited Destroy;
end;

function TIntHash.GetCount: integer;
begin
  result := length(i);
end;

function TIntHash.GetValue(ix: integer): integer;
begin
  if (ix > -1) and (ix < length(i)) then
    result := i[ix]
  else
    result := -1;
end;

end.
