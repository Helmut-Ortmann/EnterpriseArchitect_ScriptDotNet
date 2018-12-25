@echo off

REM Install the application ScriptDotNet by
REM - Copy all *.exe file and *.dll files
REM
REM This Script is called in the Post Build event
REM Example: 
REM "$(SolutionDir)ScriptCSharp\Scripts\Install.bat" "$(SolutionDir)ScriptCSharp\bin\Release" "c:\temp\EaScripts"
robocopy %1 %2 *.dll *.exe /S
exit 0