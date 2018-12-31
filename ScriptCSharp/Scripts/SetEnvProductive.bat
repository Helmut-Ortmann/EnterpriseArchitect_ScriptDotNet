@echo off

REM Set EA C# Scripting environemnt  Variable for productive use
REM Note: Only affects newly started process
REM
prompt $g
@echo on 
@echo on
rem -----------------------------------------------------------------------------
rem Productive on
rem !!!!!!Restart EA!!!!!!!
rem -----------------------------------------------------------------------------
SETX EA_SCRIPT_HOME c:\temp\EaScripts\
@echo off

pause
exit 0