@echo off

REM Delete the ScriptCSharp environemnt variable of ScriptCSharp.exe
REM
REM Use the correct path of ScriptCSharp.exe
REM Either of your IDE or your target directory

@echon on
c:\hoData\Development\GitHub\EnterpriseArchitect_ScriptDotNet\ScriptCSharp\bin\Release\ScriptCSharp.exe "DelEnv"
@echo off
pause
exit 0
