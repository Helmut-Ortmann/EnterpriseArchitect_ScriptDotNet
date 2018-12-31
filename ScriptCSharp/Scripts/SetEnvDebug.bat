@echo off

REM Set EA C# Scripting environemnt  Variable for debug use
REM Note: Only affects newly started process
REM 
prompt $g
@echo on
rem -----------------------------------------------------------------------------
rem DEBUG on
rem !!!!!!Restart EA!!!!!!!
rem -----------------------------------------------------------------------------
SETX EA_SCRIPT_HOME c:\hoData\Development\GitHub\EnterpriseArchitect_ScriptDotNet\ScriptCSharp\bin\Debug\
@echo off

pause
exit 0