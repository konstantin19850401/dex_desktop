set basep=%~dp0

del /f /q %basep%\out\archive.7z
del /f /q %basep%\out\update.exe
del /f /q %basep%\out\update.key

cd %basep%\distr\
%basep%\tools\7z.exe a %basep%\out\archive.7z -r * -t7z -mx
cd ..
copy /b %basep%\distr\update.key %basep%\out\update.key
copy /b %basep%\tools\7zSD.sfx + %basep%\tools\config.txt + %basep%\out\archive.7z %basep%\out\update.exe
del /f /q %basep%\out\archive.7z
