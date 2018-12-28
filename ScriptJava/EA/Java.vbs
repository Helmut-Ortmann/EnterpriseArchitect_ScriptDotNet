option explicit

!INC Local Scripts.EAConstants-VBScript
' Include CSharp/Java script interface
!INC ScriptDotNet.RunCommandVb 

'
' Script Name: 
' Author: 
' Purpose: 
' Date: 
'
sub main
    dim result
	result = RunCommandJava("%EA_SCRIPT_HOME%", "SparxSystems.RepositoryInterface", " ", " "," ", " ")
end sub

main