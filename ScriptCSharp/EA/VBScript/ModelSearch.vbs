option explicit

!INC Local Scripts.EAConstants-VBScript
' Include CSharp script interface
!INC ScriptDotNet.RunCommandVb 

'
' Script Name: ModelSearch
' Author:      Helmut Ortmann
' Purpose:     Show how to run C# Console Program from EA VB Script
' Date:        26. December 2018
' See: https://github.com/Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
'
sub main
    ' Do everything in C#
	' The context object contains the selected line of the ModelSearch Window
	' Note: Search has to set CLASSGUID, CLASSTYPE
	'       The symbol on the elft shows you that EA has identified the 
    runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "ModelSearch", "", ""
	
end sub

main