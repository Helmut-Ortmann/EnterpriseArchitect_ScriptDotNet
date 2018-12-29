'option explicit
'
' See:
' GitHub: Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
' - https://github.com/Helmut-Ortmann/EnterpriseArchitect_ScriptDotNet
' SPARX Webinar Hybrid Scripting
' - http://www.sparxsystems.com/resources/webinar/release/ea13/videos/hybrid-scripting.html
' SPARX Tutorial Hyper Script
' http://www.sparxsystems.com/resources/user-guides/automation/hybrid-scripting.pdf

' Geert Bellekens Tutorial to use VB Script Library: 
' https://bellekens.com/2015/12/27/how-to-use-the-enterprise-architect-vbscript-library/
'[path=\Framework\ho\run]
'[group=HybridScripting]

'--------------------------------------------------------------
' RunCommand
' Runs the passed *.exe file and returns the Standard Output
' 
' Signiture:
' result = RunCommand(command, parameter1, parameter2 parameter3)
'
' Description:
' - Estimates the own Process ID
' - Runs the designated *.exe file
' - Reads the Standard Output and returns it to the caller
'
' Improvements:
' - Use Windows %PATH% Environment Variable
' - Make a fix location structure for your Scripts
' - Use local path
' - Your ideas
' - ..
'
' Let me know about your experiences, improvements and suggestions
' Helmut.Ortmann@t-Online.de
'


!INC Local Scripts.EAConstants-VBScript

' Testfunction: 
' - Use it to test and get a basic understanding. At the end of this file you can switch the call of this Test Function on or off.
'   To do this insert or delete the apostrophe (Last line of this file/script)
sub Test
	Dim process, result
	process = ProcessId("EA.exe")
	Session.Output "--------------------------------------------"
	Session.Output "ProcessId('EA.exe')" & vbCRLF
	Session.Output process
    Session.Output "--------------------------------------------"
	' Test Run(..,..)
	result = Run("ping", "127.0.0.1", " ", " ", " ")
	Session.Output "--------------------------------------------"
	Session.Output "Run('ping', '127.0.0.1', ' ', ' ', ' ')" & vbCRLF
	Session.Output result
    Session.Output "--------------------------------------------"
	Session.Output vbCRLF & vbCRLF & vbCRLF
	' Test RunEA(..,..)
	result = RunEA("ping", "", " ")
	Session.Output "RunEA(), Result ping:" & vbCRLF & result
end sub

'---------------------------------------------------------------------------------------------------------------------------------
' Testfunction: 
' - Use it to test and get a basic understanding. At the end of this file you can switch the call of this Test Function on or off.
'   To do this insert or delete the apostrophe (Last line of this file/script)
sub TestJava
	Dim process, result
	process = ProcessId("EA.exe")
	Session.Output "--------------------------------------------"
	Session.Output "ProcessId('EA.exe')" & vbCRLF
	Session.Output process
    Session.Output "--------------------------------------------"
	' Test RunJava(..,..)
	result = RunCommandJava("c:\temp\java", "SparxSystems.RepositoryInterface", " ", " "," ", " ")
	Session.Output "--------------------------------------------"
	Session.Output "RunJava 'c:\temp\java', 'SparxSystems.RepositoryInterface', ' ', ' ', ' ', ' ')" & vbCRLF
	Session.Output result
    Session.Output "--------------------------------------------"
	Session.Output vbCRLF & vbCRLF & vbCRLF
end sub



'--------------------------------------------------------------------
' Function to call an arbitrary *.exe and return the Standard Output to the caller
'
' Parameters:
' - CommandExe   The *.exe file to call
' - param1       Your parameter 1 you want to pass to the exe, usually the function to do, e.g. "Find"
' - param2       Your parameter 2 you want to pass to the exe, usually a guid to {.......}
' - param3       Your parameter 3 you want to pass to the exe
' - Return Value The Standard Output of the called *.exe
'
' Your *.exe:    Get the EA Repository by the Process ID of the EA Instance
' - prosessId    The ProcessID of the EA Instance
' - para1        param1, usually the thing to do (e.g. "Print","ShowItem","SearchFor",..) 
' - para2        param2 
' - para3        param3 
Function RunCommand(CommandExe, param1, param2, param3)
    RunCommand = Run(CommandExe, ProcessId("EA.exe"), param1, param2, param3)
End Function


'--------------------------------------------------------------------
' Function to call an Java Class and returns the Standard Output to the caller
'
' Parameters:
' - CommandExe   The Java Class to call
' - param1       Your parameter 1 you want to pass to the exe
' - param2       Your parameter 2 you want to pass to the exe
' - param3       Your parameter 3 you want to pass to the exe
' - Return Value The Standard Output of the called *.exe
'
' Your Java Class:    Get the EA Repository by the Process ID of the EA Instance
' - para1             The ProcessID of the EA Instance
' - para2             param1 
' - para3             param2 
' - para4             param3 
Function RunCommandJava(baseFolder, eaClass, param1, param2, param3, param4)
    RunCommandJava = RunJava(baseFolder, eaClass, ProcessId("EA.exe"), param1, param2, param3)
End Function


'--------------------------------------------------------------------
' Helper to get the Process ID of the own process
Function ProcessId(strScriptName)
	Dim datHighest
	Dim lngMyProcessId
	Dim WMI, wql
	Dim objProcess
	'Initialise 
	datHighest = Cdbl(0)
	lngMyProcessId = 0

	Set WMI = GetObject("winmgmts:{impersonationLevel=impersonate}!\\.\root\cimv2")
	wql = "SELECT * FROM Win32_Process WHERE Name = '" & strScriptName & "'"
	'
	For Each objProcess In WMI.ExecQuery(wql)
	  'The next If is not necessary, it only restricts the search to all processes on the current VB Script
	  'If Instr(objProcess.CommandLine, WScript.ScriptName) <> 0 Then
		If objProcess.CreationDate > datHighest Then
		  'Take the process with the highest CreationDate so far
		  '  e.g. 20160406121130.510941+120   i.e. 2016-04-06 12h11m:30s and fraction
		  datHighest = objProcess.CreationDate
		  lngMyProcessId = objProcess.ProcessId
		End If
	  'End If
	Next
	ProcessId = lngMyProcessId
End Function

'-----------------------------------------------------
' Helper function to run an *.exe with 3 parameters
' Tested with C# in a Hybrid SPARX Environment 
' It reads the Standard Output and returns it as the result
'
' CommandExe:  Full path of the C# exe according to SPARX
' param1:      Value parameter you want to pass to CommandExe (no references, objects)
' param2:      Value parameter you want to pass to CommandExe (no references, objects)
' param3:      Value parameter you want to pass to CommandExe (no references, objects)
' param4:      Value parameter you want to pass to CommandExe (no references, objects)
Function Run(CommandExe, pid, param1, param2, param3) 
    Dim ws,wsShellExe, Command
	Dim stdOut ' Standard output
	Dim stdErr ' Error output
	Const WshFinished = 1
    Const WshFailed = 2
	
    Set ws = CreateObject("WScript.Shell")
    ' make sure the path may contain spaces
    ' use '"' to wrap opath string	
	'http://www.vbsedit.com/html/5593b353-ef4b-4c99-8ae1-f963bac48929.asp
	
	' Expand environment variables
	commandExe = ws.ExpandEnvironmentStrings(CommandExe)
    command = CommandExe & " " & pid & " "& param1 & " " & param2 & " " & param3 & " " 
	On Error Resume Next
    Set wsShellExe = ws.Exec(command)
	If Err.Number <> 0 Then
	  MsgBox "Command:'"  & vbCRLF & command & _
     	  "'" & vbCRLF & "Error:" & Err.Number & _
		  vbCRLF & "Source:" & Err.Source & _
		  vbCRLF & "Description:" & Err.Description, _
		  65, _
		  "Error running command"
	  return
	End If
	On Error Goto 0

	stdErr = wsShellExe.StdErr.ReadAll
	
	Select Case wsShellExe.Status
      Case WshFinished
		 'Session.Output "WshFinished"
		 stdOut = wsShellExe.StdOut.ReadAll
      Case WshFailed
         'strOutput = wsShellExe.StdErr.ReadAll
		 Session.Output "WshEnd"
	  Case Else
	     'Session.Output "Error"
		 stdOut = "Undefined Error!"
    End Select
    Run = stdOut
End Function


'-----------------------------------------------------
' Helper function to run Java Class with 3 parameters
' Tested with java in a Hybrid SPARX Environment 
' It reads the Standard Output and returns it as the result
' Note: Build has run
'       java sdk 32 bit (bin folder) is in path
'
' baseFolder:   Full path of the C# exe according to SPARX
' eaClass:     e.g.: SparxSystems.RepositoryInterface
' param1:      Value parameter you want to pass to CommandExe (no references, objects)
' param2:      Value parameter you want to pass to CommandExe (no references, objects)
' param3:      Value parameter you want to pass to CommandExe (no references, objects)
' param4:      Value parameter you want to pass to CommandExe (no references, objects)
'
' baseFolder\
'           \eaapi.jar
'           \SSJavaCOM.dll
'           \SparxSystems\
'                        \... your Java classes
Function RunJava(baseFolder, eaClass, param1, param2, param3, param4)
    Dim ws,wsShellExe, Command, Java32Path
    Dim objEnv
	Dim stdOut ' Standard output
	Dim stdErr ' Error output
	Const WshFinished = 1
    Const WshFailed = 2
	


	Set ws = CreateObject("WScript.Shell")
	' Expand environment variables
	baseFolder = ws.ExpandEnvironmentStrings(baseFolder)
	java32Path = ws.ExpandEnvironmentStrings("%JDK32_HOME%bin")
	Session.Output "JavaPath='" & java32Path & "'"
	Session.Output "Classes='" & baseFolder & "'"
	
    ' Set environment variable	
    Set objEnv = ws.Environment ("PROCESS")
    objEnv("PATH") = objEnv("PATH") & ";" & baseFolder
	
	' Set current folder
	ws.CurrentDirectory = baseFolder

    ' Set path environment variable for 32 bit Java	
    objEnv("PATH") = java32Path & ";" & objEnv("PATH") 

    Set ws = CreateObject("WScript.Shell")
    command = "java -cp ""eaapi.jar;.;"" " & eaClass & " " & param1 & " " &param2 & " " & param3 & " " & param4 & " " 
    Session.Output "Command=" & "'" & command & "'"
	Session.Output " "
	
    On Error Resume Next
    Set wsShellExe = ws.Exec(command)
	If Err.Number <> 0 Then
	  MsgBox "Command:'"  & vbCRLF & command & _
     	  "'" & vbCRLF & "Error:" & Err.Number & _
		  vbCRLF & "Source:" & Err.Source & _
		  vbCRLF & "Description:" & Err.Description, _
		  65, _
		  "Error running command"
	  return
	End If
	On Error Goto 0

	stdErr = wsShellExe.StdErr.ReadAll
	
	Select Case wsShellExe.Status
      Case WshFinished
		 'Session.Output "WshFinished"
		 stdOut = wsShellExe.StdOut.ReadAll
      Case WshFailed
         'strOutput = wsShellExe.StdErr.ReadAll
		 Session.Output "WshEnd"
	  Case Else
	     'Session.Output "Error"
		 stdOut = "Undefined Error!"
    End Select
    RunJava = stdOut 
 End Function

'-----------------------------------------------------------
' Test      Execute C# EA HyperScripting from EA Script GUI
' TestJava  Execute Java EA HyperScripting from EA Script GUI
'
' To use or not use this test functionality remove/insert beneath apostrophe before 'Test' or 'TestJava'
'
'Test
'TestJava