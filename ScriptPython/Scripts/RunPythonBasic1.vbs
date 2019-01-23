option explicit

!INC Local Scripts.EAConstants-VBScript
' Include ScriptDotNet
!INC ScriptDotNet.RunCommandVb

'------------------------------------------------------------------
' Just outputs some text in the Message Box
'------------------------------------------------------------------
sub main
    ' Python makes everything
	Dim output
	output = runPython("%EA_SCRIPT_HOME%RunPythonBasic1.py", "MakeSomeBasicThings", "", "", "")
	MsgBox "Output='" & vbCRLF & output, 0, "Output of RunPythonBasic1"
end sub

main