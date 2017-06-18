
@echo off

set ProjectDir=Toolbar
set Targetdir=GameData\000_Toolbar


set H=R:\KSP_1.3.0_dev
echo %H%

copy /Y %ProjectDir%\obj\Debug\aaa_Toolbar.dll %Targetdir%\Toolbar.dll



cd GameData
mkdir "%H%\GameData\000_Toolbar"
xcopy /y /s 000_Toolbar "%H%\GameData\000_Toolbar"
