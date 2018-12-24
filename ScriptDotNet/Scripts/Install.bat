@echo off

REM Install the application ScriptDotNet by
REM - Copy all *.exe file and *.dll files
robocopy %1 %2 *.dll *.exe /S
exit 0