@echo off

REM Deploy Java application to central Java Script Library
REM from ..\ScriptJava\bin\release\
REM to c:\Temp\EaScriptsJava\
REM - *.dll
REM - *.jar
REM - *.class
REM
REM Adapt to your  needs
REM 

@echo on
robocopy "c:\hoData\Development\GitHub\EnterpriseArchitect_ScriptDotNet\ScriptJava\bin\release" "c:\Temp\EaScriptsJava" *.dll *.jar *.class /S

@echo off
Pause
exit 0