option explicit

!INC Local Scripts.EAConstants-VBScript
!INC ScriptDotNet.RunCommandVb

'
' This code has been included from the default Project Browser template.
' If you wish to modify this template, it is located in the Config\Script Templates
' directory of your EA install path.   
'
' Script Name: TraversePackage
' Author:      Helmut Ortmann
' Purpose:     Show how to run C# Console Program from EA VB Script
' Date:        26. December 2018
' See: https://github.com/Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
'
'
' Project Browser Script main function
'
sub OnProjectBrowserScript()
	
	' Get the type of element selected in the Project Browser
	dim treeSelectedType
	treeSelectedType = Repository.GetTreeSelectedItemType()
	
	' Handling Code: Uncomment any types you wish this script to support
	' NOTE: You can toggle comments on multiple lines that are currently
	' selected with [CTRL]+[SHIFT]+[C].
	select case treeSelectedType
	
'		case otElement
'			' Code for when an element is selected
'			dim theElement as EA.Element
'			set theElement = Repository.GetTreeSelectedObject()
'					
		case otPackage
			' Code for when a package is selected
	        ' Run the CSharp script
		    ' Command: "ListDiagramElements"
		    ' Par1:    Package GUID
			dim thePackage as EA.Package
			set thePackage = Repository.GetTreeSelectedObject()
			runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "TraversePackage", thePackage.PackageGUID
			
'			
'		case otDiagram
'			' Code for when a diagram is selected
'			dim theDiagram as EA.Diagram
'			set theDiagram = Repository.GetTreeSelectedObject()
'			
'		case otAttribute
'			' Code for when an attribute is selected
'			dim theAttribute as EA.Attribute
'			set theAttribute = Repository.GetTreeSelectedObject()
'			
'		case otMethod
'			' Code for when a method is selected
'			dim theMethod as EA.Method
'			set theMethod = Repository.GetTreeSelectedObject()
		
		case else
			' Error message
			Session.Prompt "This script does not support items of this type.", promptOK
			
	end select
	
end sub

OnProjectBrowserScript
