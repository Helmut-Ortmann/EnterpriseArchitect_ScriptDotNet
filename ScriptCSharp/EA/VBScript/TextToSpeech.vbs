option explicit

!INC Local Scripts.EAConstants-VBScript
!INC ScriptDotNet.RunCommandVb

'
' Read the selected element aloud.
'
sub OnProjectBrowserScript()
	
	runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "TextToSpeechItem", "", ""
	
end sub

OnProjectBrowserScript
