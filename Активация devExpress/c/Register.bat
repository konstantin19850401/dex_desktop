@echo off

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" -u DevExpress.Data.v10.2
mkdir %windir%\assembly\GAC_MSIL\DevExpress.Data.v10.2\10.2.8.0__b88d1754d700e49a
copy DevExpress.Data.v10.2.dll %windir%\assembly\GAC_MSIL\DevExpress.Data.v10.2\10.2.8.0__b88d1754d700e49a

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" -u DevExpress.Utils.v10.2
mkdir %windir%\assembly\GAC_MSIL\DevExpress.Utils.v10.2\10.2.8.0__b88d1754d700e49a
copy DevExpress.Utils.v10.2.dll %windir%\assembly\GAC_MSIL\DevExpress.Utils.v10.2\10.2.8.0__b88d1754d700e49b

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" -u DevExpress.Utils.v10.2
mkdir %windir%\assembly\GAC_MSIL\DevExpress.XtraGrid.v10.2\10.2.8.0__b88d1754d700e49a
copy DevExpress.XtraGrid.v10.2.dll %windir%\assembly\GAC_MSIL\DevExpress.XtraGrid.v10.2\10.2.8.0__b88d1754d700e49b

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" -u DevExpress.XtraLayout.v10.2
mkdir %windir%\assembly\GAC_MSIL\DevExpress.XtraLayout.v10.2\10.2.8.0__b88d1754d700e49a
copy DevExpress.XtraLayout.v10.2.dll %windir%\assembly\GAC_MSIL\DevExpress.XtraLayout.v10.2\10.2.8.0__b88d1754d700e49b

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" -u DevExpress.CodeRush.Common
mkdir %windir%\assembly\GAC_MSIL\DevExpress.CodeRush.Common\10.2.8.0__35c9f04b7764aa3d
copy DevExpress.CodeRush.Common.dll %windir%\assembly\GAC_MSIL\DevExpress.CodeRush.Common\10.2.8.0__35c9f04b7764aa3d
REM copy DevExpress.CodeRush.Common.dll "C:\Program Files\DevExpress 2010.2\IDETools\System\DXCore\BIN\DevExpress.CodeRush.Common.dll"
if "[%ProgramFiles(x86)%]" == "[]" (copy DevExpress.CodeRush.Common.dll "%ProgramFiles%\DevExpress 2010.2\IDETools\System\DXCore\BIN\DevExpress.CodeRush.Common.dll") else (copy DevExpress.CodeRush.Common.dll "%ProgramFiles(x86)%\DevExpress 2010.2\IDETools\System\DXCore\BIN\DevExpress.CodeRush.Common.dll")


echo 'OK'
pause