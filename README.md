# EnterpriseArchitect_ScriptDotNet

- Use the power of
  - .NET like C#, F#, VB, C++
  - [Java](../../wiki/Java)

for your EA Scripting and Querying.

Implement your EA Scripts in .NET or Java and make only the glue-code to EA in JScript, VB Script or JavaScript. The minimal EA glue-code is responsible for EA integration and passing the parameters to your target environment (.NET or [Jave](Java)).
You use your IDE with your Debugger.

The additional effort: Three lines or so glue-code to connect to the C# Console Application which does the heavy work.

## Benefits

- Find your typos at compile time
- A vast amount of libraries, examples and tutorials
- Easy testing and debugging
- [LINQ for SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx), the powerful way of SQL

## Principle

EA:  

```vbScript
result = RunCommand(myScript.exe, "DoTask1", guid, "") ' vb script glue-code
```

.NET:

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

[Java](Java):

## EA glue-code

Take the EA-Script Template and add three or so lines of code and you have done integration or the so-called glue-code.
Decide whether you pass the environment like guid ot type to C# or let C# do it.

![EA VB Scripts](../../wiki/images/VbScriptsOverview.png)

You may condense this code section to (one line of EA VB Script code):

```vbscript
' Call C# "TraversePackage" and make everything there
runCommand "%EA_SCRIPT_HOME%ScriptCSharp.exe", "TraversePackage", "", ""
```

## References

- [EA Community](https://community.sparxsystems.com/community-resources/1065-use-c-java-for-your-vb-script), Use C#, Java, for your VB Scripting
- [EA Hybrid Scripting](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/HybridScripting)
- [Why LINQ beats SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx)
- [WiKi](../../wiki)

## History

### 1.1.0 

- VB Script: RunComand with three parameters instead of 2
- LINQ to SQL as project ScriptsCSharpLinq  (powerful, database independent)

### 1.0.0 Created

- C# Interface
- VB: Browser Script
- VB: Diagram Script
- Preparation Java Interface
