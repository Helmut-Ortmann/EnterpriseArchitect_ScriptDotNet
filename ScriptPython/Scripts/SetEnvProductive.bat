@echo off

REM Set EA Python Scripting environemnt  Variable for productive use
REM Note: Only affects newly started process
REM 
REM
prompt $g
@echo on 
@echo on
rem -----------------------------------------------------------------------------
rem Productive on
rem !!!!!!Restart EA!!!!!!!
rem -----------------------------------------------------------------------------
SETX EA_SCRIPT_HOME "c:\Temp\Python\EA\ea.py"
@echo off

pause
exit 0