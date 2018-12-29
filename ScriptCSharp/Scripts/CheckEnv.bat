@echo off

REM Check the ScriptCSharp environemnt variable of ScriptCSharp.exe
REM
REM Use the correct path of ScriptCSharp.exe
REM Either of your IDE or your target directory
@echo on
%EA_SCRIPT_HOME%/ScriptCSharp.exe "info"

@echo off
pause
exit 0