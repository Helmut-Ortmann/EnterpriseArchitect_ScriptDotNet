# EnterpriseArchitect_ScriptDotNet

Use the power of .NET like C#, F#, VB for your EA Scripting and Querying.

Implement your EA Scripts in .NET and make only the glue code to EA in JScript, VB Script or JavaScript. The minimal EA glue code is responsible for EA integration and passing the parameters to the Windows Console Application written in your favourite .NET language.

You use your IDE with your Debugger.

## Benefits

* Find your typos at compile time
* A vast amount of libraries, examples and tutorials
* Easy testing
* [LINQ for SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx) the powerful way of SQL

## Principle

EA:  

```vbScript
result = RunCommand(myScript.exe, "DoTask1", guid) ' vb script glue code
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

## References

* [EA Community](https://community.sparxsystems.com/community-resources/1065-use-c-java-for-your-vb-script), Use C#, Java, for your VB Scripting
* [EA Hybrid Scripting](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/HybridScripting)
* [Why LINQ beats SQL](https://www.linqpad.net/WhyLINQBeatsSQL.aspx)
* [WiKi](../../WiKi)
