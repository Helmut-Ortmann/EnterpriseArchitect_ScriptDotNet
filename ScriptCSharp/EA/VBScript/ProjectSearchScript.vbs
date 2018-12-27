option explicit

!INC Local Scripts.EAConstants-VBScript
' Include CSharp script interface
!INC ScriptDotNet.RunCommandVb 

'
' This code has been included from the default Search Script template.
' If you wish to modify this template, it is located in the Config\Script Templates
' directory of your EA install path.   
'
' Script Name: projectSearchScript
' Author:      Helmut Ortmann
' Purpose:     Show how to run C# Console Program from EA VB Script
' Date:        26. December 2018
' See: https://github.com/Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
'

' TODO 1: Define your search specification:
' The columns that will apear in the Model Search window
dim SEARCH_SPECIFICATION 
SEARCH_SPECIFICATION = "<ReportViewData>" &_
							"<Fields>" &_
								"<Field name=""CLASSGUID""/>" &_
								"<Field name=""CLASSTYPE"" />" &_
								"<Field name=""Element Name"" />" &_
								"<Field name=""Comments"" />" &_
							"</Fields>" &_
							"<Rows/>" &_
						"</ReportViewData>"

'
' Search Script main function
' 
sub OnSearchScript()

	runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe","ProjectSearch","",""
end sub	

'
' TODO 3: Modify this function signature to include all information required for the search
' results. Entire objects (such as elements, attributes, operations etc) may be passed in.
'
' Adds an entry to the xml row node 'rowsNode'
'
sub AddRow( xmlDOM, rowsNode, elementGUID, elementName, comments )

	' Create a Row node
	dim row
	set row = xmlDOM.createElement( "Row" )
	
	' Add the Model Search row data to the DOM
	AddField xmlDOM, row, "CLASSGUID", elementGUID
	AddField xmlDOM, row, "CLASSTYPE", "Class"
	AddField xmlDOM, row, "Name", elementName
	AddField xmlDOM, row, "Comments", comments
	
	' Append the newly created row node to the rows node
	rowsNode.appendChild( row )

end sub

'
' Adds an Element to the DOM called Field which makes up the Row data for the Model Search window.
' <Field name "" value ""/>
'
sub AddField( xmlDOM, row, name, value )

	dim fieldNode
	set fieldNode = xmlDOM.createElement( "Field" )
	
	' Create first attribute for the name
	dim nameAttribute
	set nameAttribute = xmlDOM.createAttribute( "name" )
	nameAttribute.value = name
	fieldNode.attributes.setNamedItem( nameAttribute )
	
	' Create second attribute for the value
	dim valueAttribute 
	set valueAttribute = xmlDOM.createAttribute( "value" )
	valueAttribute.value = value
	fieldNode.attributes.setNamedItem( valueAttribute )
	
	' Append the fieldNode
	row.appendChild( fieldNode )

end sub

OnSearchScript()
