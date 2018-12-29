option explicit

!INC Local Scripts.EAConstants-VBScript
' Include ScriptDotNet
!INC ScriptDotNet.RunCommandVb

sub main
    ' C# makes everything
	runCommand "%EA_SCRIPT_HOME%ScriptCSharpLinq.exe", "LinqForSql", "", ""
end sub

main