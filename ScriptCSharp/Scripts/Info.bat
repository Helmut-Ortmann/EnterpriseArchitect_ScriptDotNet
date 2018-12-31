@echo off

REM Info of ScriptCSharp.exe
REM - Copy all *.exe file and *.dll files
robocopy %1 %2 *.dll *.exe /S
exit 0