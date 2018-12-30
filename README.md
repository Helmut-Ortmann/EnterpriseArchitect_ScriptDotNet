# EnterpriseArchitect_ScriptDotNet

Use the power of

- .NET like C#, F#, VB, C++
- [Java](../../wiki/Java)

for your EA Scripting and Querying.

Implement your EA Scripts in .NET or [Java](Java) and make only the glue-code to EA in JScript, VB Script or JavaScript. 
The minimal EA glue-code is responsible for EA integration and passing the parameters to your target environment (.NET or [Java](../../wiki/Java)).

## Benefits

- Find your typos at compile time
- A vast amount of libraries, examples and tutorials
- Easy testing and debugging
- [LINQ for SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx), the powerful way of SQL

## Principle

One line of code and you are in your Java, C#, VB, F#, C++ environment!

**EA:**  VB Script glue-code

```vbScript
' Run the C#, VB, F#, C++ Console Programm,
result = RunCommand(myScript.exe, "DoTask1", guid, "", "") ' C# vb script glue-code
```

```vbScript
' Run the Java Class, let Java do everything
result = RunCommandJava("%EA_SCRIPT_HOME%", "SparxSystems.RepositoryInterface", " ", " "," ", " ") ' Java vb script glue-code
```

**.NET:** See [ScriptCSharp.cs](ScriptCSharp/CSharp/ScriptCSharp.cs)

```C#
switch (command) {  // Decide what to do
    case: "DoTask1":
       var el = _repository.GetElementByGuid (guid); // get the passed element
       _repository.ShowInProjectView(el);            // show the passed element in project browser
    break;

    case: "DoTask2":
    break;
}
```

**[Java](../../wiki/Java):** See [RepositoryInterface.java](ScriptJava/Source/RepositoryInterface.java)

```vbScript
// Insert your code snippet to handle the job at hand
```

## EA glue-code

Take the EA-Script Template and add three or so lines of code and you have done integration or the so-called glue-code.
Decide whether you pass the environment like guid ot type to C# or let C# do it.

![EA VB Scripts](../../wiki/images/VbScriptsOverview.png)

You may condense this code section to (one line of EA VB Script code):

```vbscript
' Call C# "TraversePackage" and make everything there
runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "TraversePackage", "", ""
```

## Try it

1. [Installation C#](../../wiki/Installation)
2. [Tutorial C#](../../wiki/Tutorial)
3. [Java](Java)
4. Use it for other languages, VB, F#, C++, or?

## References

- [EA Script Group Properties](https://sparxsystems.com/enterprise_architect_user_guide/14.0/automation/scripts_tab.html)
- [EA Community](https://community.sparxsystems.com/community-resources/1065-use-c-java-for-your-vb-script), Use C#, VB, F#, Java for your VB Scripting
- [SPARX Webinar Hybrid Scripting](http://www.sparxsystems.com/resources/webinar/release/ea13/videos/hybrid-scripting.html)
- [SPARX Tutorial Hybrid Scripting](http://www.sparxsystems.com/resources/user-guides/automation/hybrid-scripting.pdf)
- [Where is the exe?](https://stackoverflow.com/questions/304319/is-there-an-equivalent-of-which-on-the-windows-command-line)
- [Why LINQ beats SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx)
- [WiKi](../../wiki)

## History

### 1.2.0

### 1.1.0 

- VB Script: RunComand with three parameters instead of 2
- LINQ to SQL as project ScriptsCSharpLinq  (powerful, database independent)
- [Java](../../wiki/Java)

### 1.0.0 Created

- C# Interface
- VB: Browser Script
- VB: Diagram Script
- Preparation Java Interface
