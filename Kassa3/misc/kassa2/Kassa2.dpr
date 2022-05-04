//>> Set Kassa
//>> Title Касса 2
//>> [19.01.2004] [!] Исправлен баг с расходными кассовыми ордерами
//>> [01.09.2004] [!] Изменён алгоритм влияния на дату в форме ввода операции

program Kassa2;

uses
  Forms,
  Controls,
  only_one,
  Unit1 in 'Unit1.pas' {Form1},
  Unit2 in 'Unit2.pas' {dm: TDataModule},
  FSetup in 'FSetup.pas' {Setup},
  FEOp in 'FEOp.pas' {EOp},
  FECurs in 'FECurs.pas' {ECurs},
  FEAct in 'FEAct.pas' {EAct},
  FEAcc in 'FEAcc.pas' {EAcc},
  FOrdList in 'FOrdList.pas' {OrdList},
  FEAEd in 'FEAEd.pas' {EAEd},
  FEPrih in 'FEPrih.pas' {EPrih},
  FEAgentDic in 'FEAgentDic.pas' {EAgentDic},
  FECln in 'FECln.pas' {ECln},
  FESvParam in 'FESvParam.pas' {ESvParam},
  FESvodka in 'FESvodka.pas' {ESvodka},
  FESplash in 'FESplash.pas' {Splash},
  FEUserEd in 'FEUserEd.pas' {eUserEd},
  FELoginEd in 'FELoginEd.pas' {eLoginEd},
  FEDataExport in 'FEDataExport.pas' {EDataExport},
  FEDataImport in 'FEDataImport.pas' {EDataImport};

{$R *.RES}

begin
  dm.prestartup;
  if not init_mutex(datadir) then exit;
  Application.Initialize;
  {$IFOPT D-}
  Splash:=tsplash.create(application);
  splash.show;
  {$ENDIF}
  application.processmessages;
  Application.Title := 'Касса 2 - ';
  Application.CreateForm(TForm1, Form1);
  Application.CreateForm(Tdm, dm);
  Application.CreateForm(TSetup, Setup);
  Application.CreateForm(TEOp, EOp);
  Application.CreateForm(TECurs, ECurs);
  Application.CreateForm(TEAct, EAct);
  Application.CreateForm(TEAEd, EAEd);
  Application.CreateForm(TEPrih, EPrih);
  Application.CreateForm(TEAgentDic, EAgentDic);
  Application.CreateForm(TECln, ECln);
  Application.CreateForm(TESvParam, ESvParam);
  Application.CreateForm(TeUserEd, eUserEd);
  Application.CreateForm(TeLoginEd, eLoginEd);
  Application.CreateForm(TEDataExport, EDataExport);
  Application.CreateForm(TEDataImport, EDataImport);
  {$IFOPT D-}
  splash.Hide;
  {$ENDIF}
  if (eLoginEd.ShowModal<>mrok) or (User.userid<0) then application.Terminate;
  {$IFOPT D-}
  splash.show;
  {$ENDIF}
  application.ProcessMessages;
  dm.Startup;
  {$IFOPT D-}
  splash.free;
  {$ENDIF}
  Application.Run;
end.
