option explicit

!INC Local Scripts.EAConstants-VBScript
!INC ScriptDotNet.RunCommandVb

'
' This code has been included from the default Diagram Script template.
' If you wish to modify this template, it is located in the Config\Script Templates
' directory of your EA install path.
'
' Script Name: ListDiagramElements
' Author:      Helmut Ortmann
' Purpose:     Show how to run C# Console Program from EA VB Script
' Date:        26. December 2018
' See: https://github.com/Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
'

'
' Diagram Script main function
'
sub OnDiagramScript()

	' Get a reference to the current diagram
	dim currentDiagram as EA.Diagram
	set currentDiagram = Repository.GetCurrentDiagram()
	
	if not currentDiagram is nothing then 
	    ' Run the CSharp script
		' Command: "ListDiagramElements"
		' Par1:    Diagram GUID
		runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "ListDiagramElements", currentDiagram.DiagramGUID, " "

	
		' Get a reference to any selected connector/objects
		dim selectedConnector as EA.Connector
		dim selectedObjects as EA.Collection
		set selectedConnector = currentDiagram.SelectedConnector
		set selectedObjects = currentDiagram.SelectedObjects

		if not selectedConnector is nothing then
			' A connector is selected
		elseif selectedObjects.Count > 0 then
			' One or more diagram objects are selected
		else
			' Nothing is selected
		end if
	else
		Session.Prompt "This script requires a diagram to be visible", promptOK
	end if

end sub

OnDiagramScript
