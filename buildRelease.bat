
@echo off
set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"


set ProjectDir=Toolbar\
set Targetdir=GameData\000_Toolbar\

xcopy /Y %ProjectDir%etc\*.png %TargetDir%
xcopy /Y %ProjectDir%etc\Toolbar.version %TargetDir%
xcopy /Y %ProjectDir%etc\MiniAVC.dll %TargetDir%
copy /Y  %ProjectDir%\obj\Debug\aaa_Toolbar.dll %TargetDir%\Toolbar.dll


copy GameData\000_Toolbar\Toolbar.version a.version
set VERSIONFILE=a.version
rem The following requires the JQ program, available here: https://stedolan.github.io/jq/download/
c:\local\jq-win64  ".VERSION.MAJOR" %VERSIONFILE% >tmpfile
set /P major=<tmpfile

c:\local\jq-win64  ".VERSION.MINOR"  %VERSIONFILE% >tmpfile
set /P minor=<tmpfile

c:\local\jq-win64  ".VERSION.PATCH"  %VERSIONFILE% >tmpfile
set /P patch=<tmpfile

c:\local\jq-win64  ".VERSION.BUILD"  %VERSIONFILE% >tmpfile
set /P build=<tmpfile
del tmpfile
set VERSION=%major%.%minor%.%patch%
if "%build%" NEQ "0"  set VERSION=%VERSION%.%build%
echo %VERSION%

del a.version

set FILE="%RELEASEDIR%\Toolbar-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% GameData

pause