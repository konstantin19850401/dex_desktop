unit FEAEd;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, RXCtrls, ComCtrls, adsrelate, sysrelate, DB, adsdata,
  adsfunc, adstable;

type
  TEAEd = class(TForm)
    Label1: TLabel;
    eAcc: TEdit;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label11: TLabel;
    Label12: TLabel;
    eOwnCompanyName: TEdit;
    eGlavBuh: TEdit;
    eKassir: TEdit;
    eDirTitle: TEdit;
    eDirName: TEdit;
    PageControl1: TPageControl;
    ts41: TTabSheet;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    Label10: TLabel;
    Label13: TLabel;
    rsbShowPrihod: TRxSpeedButton;
    epOKUD: TEdit;
    epOKPO: TEdit;
    epDEBET: TEdit;
    epSPODR: TEdit;
    epKSS: TEdit;
    epKANAL: TEdit;
    epKCEL: TEdit;
    ts42: TTabSheet;
    Label14: TLabel;
    Label15: TLabel;
    Label16: TLabel;
    Label17: TLabel;
    Label18: TLabel;
    Label19: TLabel;
    Label20: TLabel;
    rsbShowRashod: TRxSpeedButton;
    erOKUD: TEdit;
    erOKPO: TEdit;
    erKREDIT: TEdit;
    erSPODR: TEdit;
    erKSS: TEdit;
    erKANAL: TEdit;
    erKCEL: TEdit;
    rsbOK: TRxSpeedButton;
    rsbCancel: TRxSpeedButton;
    Label21: TLabel;
    Label22: TLabel;
    eAgent: TEdit;
    Label23: TLabel;
    ePassport: TEdit;
    RxSpeedButton1: TRxSpeedButton;
    procedure FormShow(Sender: TObject);
    procedure rsbOKClick(Sender: TObject);
    procedure rsbShowPrihodClick(Sender: TObject);
    procedure rsbCancelClick(Sender: TObject);
    procedure rsbShowRashodClick(Sender: TObject);
    procedure RxSpeedButton1Click(Sender: TObject);
  private
    procedure getvalues(s: tstringlist);
    { Private declarations }
  public
    { Public declarations }
    id: integer;
  end;

var
  EAEd: TEAEd;

implementation

uses Unit2, FEAgentDic;

{$R *.dfm}

procedure TEAEd.FormShow(Sender: TObject);
var
    s:tstringlist;
    q:tadsquery;
begin
    eAcc.text:=''; eOwnCompanyName.Text:='';
    eDirTitle.text:=''; eGlavBuh.Text:='';
    eKassir.text:=''; eDirName.text:='';

    epOKUD.Text:=''; epOKPO.text:='';
    epDEBET.Text:=''; epSPODR.Text:='';
    epKSS.text:=''; epKANAL.text:='';
    epKCEL.Text:='';

    erOKUD.Text:=''; erOKPO.text:='';
    erKREDIT.Text:=''; erSPODR.Text:='';
    erKSS.text:=''; erKANAL.text:='';
    erKCEL.Text:='';
    eAgent.text:=''; ePassport.text:='';
    
    if id>-1 then begin
        q:=adsq(datadir,'select * from "'+acc_name+'" where ID='+inttostr(id));
        if (q<>nil) then begin
            if (q.active) and (q.recordcount>0) then begin
                s:=tstringlist.Create;
                s.Text:=q.fieldbyname('data').asstring;
                eAcc.text:=q.fieldbyname('Name').asstring;
                eowncompanyname.Text:=getvalue(s,'owncompanyname','');
                eDirTitle.text:=getvalue(s,'DirTitle','');
                eGlavBuh.Text:=getvalue(s,'glavbuh','');
                eKassir.text:=getvalue(s,'kassir','');
                eDirName.text:=getvalue(s,'dirname','');

                epOKUD.Text:=getvalue(s,'epokud','');
                epOKPO.text:=getvalue(s,'epokpo','');
                epDEBET.Text:=getvalue(s,'epdebet','');
                epSPODR.Text:=getvalue(s,'epspodr','');
                epKSS.text:=getvalue(s,'epkss','');
                epKANAL.text:=getvalue(s,'epkanal','');
                epKCEL.Text:=getvalue(s,'epkcel','');

                erOKUD.Text:=getvalue(s,'erokud','');
                erOKPO.text:=getvalue(s,'erokpo','');
                erKREDIT.Text:=getvalue(s,'erkredit','');
                erSPODR.Text:=getvalue(s,'erspodr','');
                erKSS.text:=getvalue(s,'erkss','');
                erKANAL.text:=getvalue(s,'erkanal','');
                erKCEL.Text:=getvalue(s,'erkcel','');
                eAgent.Text:=getvalue(s,'Agent','');
                ePassport.Text:=getvalue(s,'Passport','');
            end;
            q.close;
            q.free;
        end;
    end;
    eacc.setfocus;
end;

procedure teaed.getvalues(s:tstringlist);
begin
    setvalue(s,'owncompanyname',eowncompanyname.Text);
    setvalue(s,'DirTitle',eDirTitle.text);
    setvalue(s,'glavbuh',eGlavBuh.Text);
    setvalue(s,'kassir',eKassir.text);
    setvalue(s,'dirname',eDirName.text);

    setvalue(s,'epokud',epOKUD.Text);
    setvalue(s,'epokpo',epOKPO.text);
    setvalue(s,'epdebet',epDEBET.Text);
    setvalue(s,'epspodr',epSPODR.Text);
    setvalue(s,'epkss',epKSS.text);
    setvalue(s,'epkanal',epKANAL.text);
    setvalue(s,'epkcel',epKCEL.Text);

    setvalue(s,'erokud',erOKUD.Text);
    setvalue(s,'erokpo',erOKPO.text);
    setvalue(s,'erkredit',erKREDIT.Text);
    setvalue(s,'erspodr',erSPODR.Text);
    setvalue(s,'erkss',erKSS.text);
    setvalue(s,'erkanal',erKANAL.text);
    setvalue(s,'erkcel',erKCEL.Text);
    setvalue(s,'Agent',eAgent.Text);
    setvalue(s,'Passport',ePassport.text);
end;

procedure TEAEd.rsbOKClick(Sender: TObject);
var
    er:string;
    q:tadsquery;
    s:tstringlist;
// SyncMod 04.05.2008
    ts: string;
// SyncMod /

begin
    er:='';
    if trim(eacc.text)='' then er:=er+'Наименование счёта не может быть пустым'#13;
    q:=adsq(datadir,'select * from "'+acc_name+'" where Name='+quotedstr(eacc.text)+
            'and ID<>'+inttostr(id));
    if (q<>nil) then begin
        if (q.active) and (q.recordcount>0) then er:=er+'Счёт с таким наименованием уже есть'#13;
        q.close;
        q.free;
    end;
    if er='' then begin
        s:=tstringlist.create;
        getvalues(s);
        if id>-1 then
// SyncMod 04.05.2008
          adse(datadir, format('update "%s" set name=%s, data=%s, r_ch=%s '+
            'where id=%d', [acc_name, quotedstr(eacc.text), quotedstr(s.Text),
            quotedstr(GetTimeStamp), id]))
// SyncMod /

//            adse(datadir,'update "'+acc_name+'" set name='+quotedstr(eacc.text)+
//                 ', Data='+quotedstr(s.Text)+' where id='+inttostr(id))
        else
// SyncMod 04.05.2008
        begin
          ts := GetTimeStamp;
          adse(datadir, format('insert into "%s" (Name, Data, r_cr, r_ch) ' +
            'values (%s, %s, %s, %s)', [acc_name, quotedstr(eacc.Text),
            quotedstr(s.Text), quotedstr(ts), quotedstr(ts)]));
        end;
// SyncMod /
//            adse(datadir,'insert into "'+acc_name+'" (Name, Data) values ('+
//                 quotedstr(eacc.text)+', '+quotedstr(s.text)+')');
        s.free;
        modalresult:=mrok;
    end else showmessage('Ошибки:'#13#13+er);
end;

procedure TEAEd.rsbCancelClick(Sender: TObject);
begin
    modalresult:=mrcancel;
end;

procedure TEAEd.rsbShowPrihodClick(Sender: TObject);
var
    s:tstringlist;
begin
    s:=tstringlist.create;
    getvalues(s);
    dm.SetReportGlobals(dm.frPrihod,s);
    dm.frPrihod.ShowReport;
end;

procedure TEAEd.rsbShowRashodClick(Sender: TObject);
var
    s:tstringlist;
begin
    s:=tstringlist.create;
    getvalues(s);
    dm.SetReportGlobals(dm.frRashod,s);
    dm.frRashod.ShowReport;
end;

procedure TEAEd.RxSpeedButton1Click(Sender: TObject);
begin
    eagentdic.ePassport.Text:=epassport.text;
    eagentdic.eagent.text:=eagent.text;
    if eagentdic.ShowModal=mrok then begin
        eagent.Text:=eagentdic.eagent.text;
        epassport.text:=eagentdic.epassport.Text;
        if (eagentdic.qa.Active) and (eagentdic.qa.RecordCount>0) and
           (eagentdic.qa.FieldByName('id').asinteger=eagentdic.id) and
           (ansisametext(eagentdic.qa.fieldbyname('c_name').asstring,
           eagentdic.eAgent.Text)) then
// SyncMod 04.05.2008
          adse(datadir, format('update "%s" set c_passport=%s, r_ch=%s ' +
            'where id=%d', [cli_name, quotedstr(eagentdic.ePassport.Text),
            quotedstr(GetTimeStamp), eagentdic.id]));
// SyncMod /

//           adse(datadir,'update "'+cli_name+'" set c_passport='+
//                quotedstr(eagentdic.ePassport.Text)+' where id='+
//                inttostr(eagentdic.id));
    end;
end;

end.
