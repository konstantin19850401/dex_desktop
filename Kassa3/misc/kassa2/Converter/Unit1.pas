unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, DBTables, DB, adsdata, adsfunc, adstable, adsrelate, sqlrelate,
  StdCtrls, Mask, ToolEdit, sysrelate, dlgrelate;

const
    ops_name='operations.adt';
    bmk_name='bookmarks.adt';
    cli_name='clients.adt';
    kas_name='kassa.adt';
    acc_name='accounts.adt';
    ord_name='orders.adt';
    svo_name='svodki.adt';
    yn:array[false..true] of string=('false','true');

type
  TSopRec=record
    ID,OldID: integer;
  end;

  TSopArray=array of TSopRec;

  TForm1 = class(TForm)
    aq: TAdsQuery;
    bq: TQuery;
    Label1: TLabel;
    Label2: TLabel;
    deSRC: TDirectoryEdit;
    deDST: TDirectoryEdit;
    Button1: TButton;
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}
procedure check_db(ddir:string);
var
    sl:tstringlist;
begin
    sl:=tstringlist.create;
    sl.text:='Table-name='+ops_name+#13+
             'Fields=*ID autoinc|*Name char(240)|optype logical|'+
             'deleted logical|ns logical|bs logical|us logical|cs logical|'+
             'nd logical|bd logical|ud logical|cd logical';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+bmk_name+#13+
             'Fields=*ID autoinc|*B_Date date|*B_Title char(128)|B_Desc memo';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+cli_name+#13+
             'Fields=*ID autoinc|*C_Name char(240)|*C_Passport char(240)|'+
             '*C_Group char(32)';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+kas_name+#13+
             'Fields=*ID autoinc|*Op_ID integer|*Prim char(240)|*K_Date Date|'+
             '*K_Sum curdouble|*K_USD curdouble|*SrcAcc integer|*DstAcc integer|'+
             '*SrcType integer|*DstType integer|*sn curdouble|*sb curdouble|'+
             '*su curdouble|*sc curdouble|*dn curdouble|*db curdouble|'+
             '*du curdouble|*dc curdouble|*bmk_id integer';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+acc_name+#13+
             'Fields=*ID autoinc|*Name char(240)|Data memo';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+ord_name+#13+
             'Fields=*ID autoinc|*JID integer|*AccID integer|*DocNum integer|'+
             '*DocDate date|*DocType integer|*Src char(128)|*Dst char(128)|'+
             '*Subj char(128)|Docsum curdouble|docusd logical|Agent char(240)|'+
             'Passport char(240)';
    CreateTableFromDef(sl,ddir);
    sl.text:='Table-name='+svo_name+#13+
             'Fields=*ID autoinc|*svx integer|*svname char(128)|data memo';
    CreateTableFromDef(sl,ddir);
    sl.free;
end;

procedure TForm1.Button1Click(Sender: TObject);
var
    units, Ops, Clients: TSopArray;
    srcb,dstb: string;
    sho:tform;
    srcacc,dstacc,opid,clid,f: integer;

function nid(var oa:tsoparray; oid: integer): integer;
var
    g:integer;
begin
    result:=-1;
    for g:=0 to length(oa)-1 do
        if oa[g].OldID=oid then begin
            result:=oa[g].ID;
            exit;
        end;
end;

procedure SCap(ss: string);
begin
    caption:=ss;
    application.processmessages;
end;

begin
    sho:=showwaitmsg('Конвертирование БД');
    srcb:=includetrailingbackslash(deSRC.text);
    dstb:=excludetrailingbackslash(deDST.text);
    scap('Create DB');
    deleteall(dstb);
    check_db(dstb);

    scap('Units');
    execdbsql(bq,srcb,'select * from units',true);
    setlength(units,0);
    if (bq.Active) and (bq.recordcount>0) then begin
        setlength(units,bq.RecordCount);
        bq.First;
        f:=1;
        while not bq.Eof do begin
            adse(dstb,'insert into "'+acc_name+'" (name) values ('+
                 quotedstr(bq.fieldbyname('name').AsString)+')');
            units[f-1].ID:=f;
            units[f-1].OldID:=bq.fieldbyname('tag').AsInteger;
            inc(f);
            bq.Next;
        end;
        bq.close;
    end;

    scap('Operations');
    execdbsql(bq,srcb,'select * from operations',true);
    setlength(ops,0);
    if (bq.Active) and (bq.recordcount>0) then begin
        setlength(ops,bq.RecordCount);
        bq.First;
        f:=1;
        while not bq.Eof do begin
            adse(dstb,'insert into "'+ops_name+
                 '" (name,optype,deleted,ns,bs,us,cs,nd,bd,ud,cd) values ('+
                 quotedstr(bq.fieldbyname('name').AsString)+', '+
                 yn[bq.fieldbyname('optype').asboolean]+', false, '+
                 yn[bq.fieldbyname('ns').asboolean]+', '+
                 yn[bq.fieldbyname('bs').asboolean]+', '+
                 yn[bq.fieldbyname('us').asboolean]+', '+
                 yn[bq.fieldbyname('cs').asboolean]+', '+
                 yn[bq.fieldbyname('nd').asboolean]+', '+
                 yn[bq.fieldbyname('bd').asboolean]+', '+
                 yn[bq.fieldbyname('ud').asboolean]+', '+
                 yn[bq.fieldbyname('cd').asboolean]+')');
            ops[f-1].ID:=f;
            ops[f-1].OldID:=bq.fieldbyname('Operation_ID').AsInteger;
            inc(f);
            bq.Next;
        end;
        bq.close;
    end;

    scap('Clients');
    execdbsql(bq,srcb,'select * from clients',true);
    setlength(Clients,0);
    if (bq.Active) and (bq.recordcount>0) then begin
        setlength(Clients,bq.RecordCount);
        bq.First;
        f:=1;
        while not bq.Eof do begin
            adse(dstb,'insert into "'+cli_name+'" (c_name, c_group) values ('+
                 quotedstr(bq.fieldbyname('name').AsString)+', '+
                 quotedstr(inttostr(bq.fieldbyname('Group').asinteger))+')');
            Clients[f-1].ID:=f;
            Clients[f-1].OldID:=bq.fieldbyname('Client_ID').AsInteger;
            inc(f);
            bq.Next;
        end;
        bq.close;
    end;

    scap('Kassa');
    execdbsql(bq,srcb,'select * from kassa',true);
    if (bq.active) and (bq.recordcount>0) then begin
        bq.first;
        while not bq.eof do begin
            case bq.fieldbyname('SrcType').AsInteger of
                0..2: srcacc:=nid(Units,bq.fieldbyname('SrcAcc').AsInteger);
                3: srcacc:=nid(Clients,bq.fieldbyname('SrcAcc').AsInteger);
            end;
            case bq.fieldbyname('DstType').AsInteger of
                0..2: dstacc:=nid(Units,bq.fieldbyname('DstAcc').AsInteger);
                3: dstacc:=nid(Clients,bq.fieldbyname('DstAcc').AsInteger);
            end;
            opid:=nid(ops,bq.fieldbyname('Operation_ID').AsInteger);
            if (srcacc>-1) and (dstacc>-1) and (opid>-1) then
                adse(dstb,'insert into "'+kas_name+
                     '" (Op_ID, Prim, K_Date, K_Sum, K_Usd, SrcAcc, DstAcc, '+
                     'SrcType, DstType, sn, sb, su, sc, dn, db, du, dc, bmk_id) values ('+
                     inttostr(opid)+', '+quotedstr(bq.fieldbyname('Prim').AsString)+
                     ', '+quotedstr(datetostr(bq.fieldbyname('Date').AsDateTime))+
                     ', '+f2s(bq.fieldbyname('Sum').AsCurrency)+', '+
                     f2s(bq.fieldbyname('Usd').AsCurrency)+', '+
                     inttostr(srcacc)+', '+inttostr(dstacc)+', '+
                     inttostr(bq.fieldbyname('SrcType').AsInteger)+', '+
                     inttostr(bq.fieldbyname('DstType').AsInteger)+', '+
                     f2s(bq.fieldbyname('sn').AsCurrency)+', '+
                     f2s(bq.fieldbyname('sb').AsCurrency)+', '+
                     f2s(bq.fieldbyname('su').AsCurrency)+', '+
                     f2s(bq.fieldbyname('sc').AsCurrency)+', '+
                     f2s(bq.fieldbyname('dn').AsCurrency)+', '+
                     f2s(bq.fieldbyname('db').AsCurrency)+', '+
                     f2s(bq.fieldbyname('du').AsCurrency)+', '+
                     f2s(bq.fieldbyname('dc').AsCurrency)+', -1)');
            bq.next;
        end;
    end;
    scap('');
    sho.free;
end;

end.
