@echo off

REM Set EA Java Scripting environment  Variables for productive use
REM - EA_SCRIPT_HOME  Classpath
REM - JDK32_HOME      32 bit JDK 
REM Note: Only affects newly started process, restart EA to be sure!
REM 
@echo on
echo %EA_SCRIPT_HOME%

SETX EA_SCRIPT_HOME c:\hoData\Development\GitHub\EnterpriseArchitect_ScriptDotNet\ScriptJava\bin\release\
SETX JDK32_HOME "c:\Program Files (x86)\Java\jdk1.8.0_191\\"
@echo off
REM
REM If yout want to work with EA Hybrid Scripting Environment you have to
REM - Add Java JDK 32 bit to windows path environemnt variable
REM - Example: 
REM c:\Program Files (x86)\Java\jdk1.8.0_191\bin\
Pause
exit 0