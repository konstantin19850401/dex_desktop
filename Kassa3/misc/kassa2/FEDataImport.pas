unit FEDataImport;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, EcXmlParser;

type
  TEDataImport = class(TForm)
    lbLog: TListBox;
    bSelectFile: TButton;
    od: TOpenDialog;
    procedure bSelectFileClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    procedure _log(s: string);
  end;

var
  EDataImport: TEDataImport;

implementation

uses FSetup;

{$R *.dfm}

{ TEDataImport }

procedure TEDataImport._log(s: string);
begin
  lbLog.ItemIndex := lbLog.Items.Add(s);
end;

procedure TEDataImport.bSelectFileClick(Sender: TObject);
const
  reqfld: array[0..20] of string = ('op_id', 'k_date', 'k_sum', 'k_usd', 'srcacc',
    'dstacc', 'srctype', 'dsstype', 'sn', 'sb', 'su', 'sc', 'dn', 'db', 'du',
    'dc', 'bmk_id', 'user_cr', 'user_ch', 'r_cr', 'r_ch');

var
  xml: tecxmlparser;
  s: string;
  i, ikas, iops, iacc, icli, iusr: txmlitem;
  f, g: integer;
  correct: boolean;

function incorrect(sinc: string): boolean;
begin
  _log('Ошибка: ' + sinc);
  result := false;
end;

function FindItem(t: txmlitem; itemname, field, value: string): txmlitem;
var
  lf: integer;
begin
  result := nil;
  if (t <> nil) and (t.SubItemCount > 0) then
  begin
    for lf := 0 to t.SubItemCount - 1 do
    begin
      if (ansisametext(t.SubItems[lf].Name, itemname)) and
         (t.SubItems[lf].Params.IndexOfName(field) > -1) and
         (ansisametext(t.SubItems[lf].Params.Values[field], value)) then
      begin
        result := t.SubItems[lf];
        break;
      end;
    end;
  end;
end;

begin
  if (od.Execute) and (fileexists(od.FileName)) then
  begin
    xml := tecxmlparser.create(nil);

    try
      xml.LoadFromFile(od.FileName);

      // Проверка корректности XML
      _log('Проверка корректности XML');
      correct := true;
      if not ansisametext(xml.Root.Name, 'Document') then
        correct := incorrect('Некорректный документ <Document>');

      if not ansisametext(xml.Root.Params.Values['type'], 'kassa') then
        correct := incorrect('Некорректный документ <type>');

      if trim(xml.Root.Params.Values['id']) = '' then
        correct := incorrect('Отсутствует идентификатор <id> БД')
      else
        if not ansisametext(xml.Root.Params.Values['id'], Setup.eBaseID.Text) then
          correct := incorrect('Документ для другой базы <' + xml.Root.Params.Values['id'] + '>');

      i := nil;
      ikas := nil;
      iops := nil;
      iacc := nil;
      icli := nil;
      iusr := nil;

      if correct then
      begin
        for f := 0 to xml.root.SubItemCount - 1 do
        begin
          i := xml.Root.SubItems[f];
          if ansisametext(i.Name, 'table') then
          begin
            if ansisametext(i.Params.Values['name'], 'kassa') then
              ikas := i
            else if ansisametext(i.Params.Values['name'], 'users') then
              iusr := i
            else if ansisametext(i.Params.Values['name'], 'clients') then
              icli := i
            else if ansisametext(i.Params.Values['name'], 'accounts') then
              iacc := i
            else if ansisametext(i.Params.Values['name'], 'operations') then
              iops := i;
          end;
        end;
        if ikas = nil then
          correct := incorrect('Таблица <kassa> отсутствует в файле импорта');

        if iusr = nil then
          correct := incorrect('Таблица <users> отсутствует в файле импорта');

        if icli = nil then
          correct := incorrect('Таблица <clients> отсутствует в файле импорта');

        if iacc = nil then
          correct := incorrect('Таблица <accounts> отсутствует в файле импорта');

        if iops = nil then
          correct := incorrect('Таблица <operations> отсутствует в файле импорта');
      end;


      if correct then
      begin
        _log('Импорт записей');

        //TODO: Импорт таблицы kassa
        for f := 0 to ikas.SubItemCount - 1 do
        begin
          i := ikas.SubItems[f];

          // Проверка целостности записи
          correct := true;
          if ansisametext(i.Name, 'Record') then
          begin
            s := '';
            for g := 0 to length(reqfld) - 1 do
            begin
              if i.Params.IndexOfName(reqfld[f]) < 0 then
              begin
                if s <> '' then
                  s := s + ', ';
                s := s + '<' + reqfld[f] + '>';
              end;
            end;
            if s <> '' then
              correct := incorrect('В записи отсутствуют поля: ' + s);
          end
          else
            correct := incorrect('Запись неизвестного типа');

          if correct then
          begin
            //Проверка смежных полей

          end;

          if correct then
          begin
            //Импорт записи

          end
          else
            _log(i.AsString);




        end;
      end;



      i := nil;
      ikas := nil;
      iops := nil;
      iacc := nil;
      icli := nil;
      iusr := nil;

    except
      on e: exception do
      begin
        _log('Внутренняя ошибка.');
        _log('Класс: ' + e.ClassName);
        _log('Сообщение: ' + e.Message);
      end;
    end;

    xml.free;
  end;
end;

end.
