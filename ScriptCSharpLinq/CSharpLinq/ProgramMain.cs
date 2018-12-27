using System;
using System.Reflection;
using System.Windows.Forms;

namespace ScriptsCSharpLinq.CSharpLinq
{
    class ProgramMain
    {
        private static readonly string EaScriptHomeEnvName = "EA_SCRIPT_HOME";
        private const string Tab = "    ";
        private static readonly bool verbose = true;
        /// <summary>
        /// Entry of the ScriptCSharpLinq.exe to be called from EA Script (JScript, JavaScript, VB Script)
        /// args[0]  PID: Process id connect to the calling EA repository
        /// args[1]  Command to execute
        /// args[2-] Parameter used to execute. Typically guid(s) and EA types
        /// 
        /// The folder vb contains the VBScript source file to call this C# exe from EA VBScript
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Standard Output or Error Output</returns>
        static void Main(string[] args)
        {
            #if DEBUG
                MessageBox.Show(@"Debug: VS

- VS: Attach to process, choose ScriptCSharpLinq
- VS: Set breakpoints
- VS: Debug
- EA: click on OK", "DEBUG: Attach VS to ScriptCSharpLinq");
            #endif
            // handle admin requests
            if (HandleAdminRequest(args)) return;

            ScriptCSharpLinq scriptCSharpLinq = GetScriptCSharp(args);
            string command = GetCommand(args);
            if (scriptCSharpLinq != null && !String.IsNullOrWhiteSpace(command))
            {
                // Print Script and its first two parameter
                if (verbose) scriptCSharpLinq.Print($"{GetScriptFullName()} '{command}' '{GetArg(args,2)}'");
                bool returnValue;
                switch (command)
                {
                    case "LinqForSql":
                        returnValue = scriptCSharpLinq.LinqForSql(args);
                        break;
                    

                    case "SetEnvHome":
                        returnValue = SetUserScriptHomeEnv();
                        break;
                    case "DelEnvHome":
                        returnValue = DelUserScriptHomeEnv();
                        break;
                    default:
                        MessageBox.Show($@"Command:{Tab}command
Par2:{Tab}{(args.Length > 1 ? args[1]:"")}
Par3:{Tab}{(args.Length > 2 ? args[2]: "")}
Par4:{Tab}{(args.Length > 3 ? args[3]: "")}
", "Command not implemented");
                        returnValue = false;
                        break;
                }
                // handle return code
                if (returnValue) ReturnOk();
                ReturnError();
            }
            else
            {
                ReturnError();
            }
        }
        /// <summary>
        /// Get the CSharp script environment
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static ScriptCSharpLinq GetScriptCSharp(string[] args)
        {
            if (args.Length > 0)
            {
                if (!Int32.TryParse(args[0], out int pid))
                {
                    MessageBox.Show($"pid: {args[0]}", "Can't read pid (process id) from the first parameter");
                    return null;
                }

                return new ScriptCSharpLinq(pid);
            }
            else
            {
                MessageBox.Show("No first parameter available. Process ID required", "Can't read pid (process id) from the first parameter");
                return null;

            }

        }
        /// <summary>
        /// Gets the command from args
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Command or "" if no command available</returns>
        static string GetCommand(string[] args)
        {
            if (args.Length > 1) return args[1];
            string msg = "No second parameter with 'command' available, break!!!";
            MessageBox.Show(msg, "Can't read the command to execute");
            Console.WriteLine(msg);
            return "";
        }

        /// <summary>
        /// Get the passed argument with the index. Blank it parameter not exists.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static string GetArg(string[] args, int index)
        {
            return args.Length > index ? args[index] : "";
        }

        /// <summary>
        /// Return error. Returns the optional message and ends the output with $"{Newline}Error"
        /// </summary>
        /// <param name="msg"></param>
        private static void ReturnError(string msg = "")
        {
            Console.WriteLine($"{msg}{Environment.NewLine}Error");
        }
        /// <summary>
        /// Returns the optional message and ends the output with $"{Newline}Ok"
        /// </summary>
        /// <param name="msg"></param>
        // ReSharper disable once UnusedParameter.Local
        private static void ReturnOk(string msg = "")
        {
            Console.WriteLine("{msg}{Environment.NewLine}Ok");
        }
        /// <summary>
        /// Handle admin requests
        /// </summary>
        /// <param name="args"></param>
        /// <returns>TRUE: Handled admin request, FALSE: not handled admin requests</returns>
        private static bool HandleAdminRequest(string[] args)
        {
            if (args.Length == 0)
            {
                GetInfo();
                return true;
            }

            if (args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    // ReSharper disable once StringLiteralTypo
                    case "setenv":
                        SetUserScriptHomeEnv();
                        return true;

                    // ReSharper disable once StringLiteralTypo
                    case "delenv":
                        DelUserScriptHomeEnv();
                        return true;

                    // ReSharper disable once StringLiteralTypo
                    case "getenv":
                        GetUserScriptHomeEnv();
                        return true;

                    case "info":
                        GetInfo();
                        return true;


                }

            }

            // no handled administration request
            return false;
        }
        /// <summary>
        /// Get info of ScriptCSharpLinq
        /// </summary>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static bool GetInfo()
        {
            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envVariable = Environment.GetEnvironmentVariable(EaScriptHomeEnvName, EnvironmentVariableTarget.User)??"";

            MessageBox.Show($@"Version:{Tab}'{Assembly.GetEntryAssembly().GetName().Version}'
Path:{Tab}{GetScriptFullName()}
Current Env:{Tab}'{EaScriptHomeEnvName}'={envVariable}
Expected Env:{Tab}'{EaScriptHomeEnvName}'={pathDirectory}
Equal Env:{Tab}{envVariable.ToLower() == pathDirectory.ToLower()}

Admin Commands:
'Info'{Tab}show this information
'GetEnv'{Tab}Get the '{EaScriptHomeEnvName}' Env Variable
'SetEnv'{Tab}Set the '{EaScriptHomeEnvName}' Env Variable with the own directory
'DelEnv'{Tab}Del the '{EaScriptHomeEnvName}' Env Variable

Copy  with CTRL+C to Clipboard, ignore beep!
", "ScriptCSharpLinq: Info");
            return true;
        }

        /// <summary>
        /// Get the EA_SCRIPT_HOME environment variable with the current path
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static bool GetUserScriptHomeEnv()
        {
            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envVariable = Environment.GetEnvironmentVariable(EaScriptHomeEnvName,  EnvironmentVariableTarget.User)??"";

            if (String.IsNullOrWhiteSpace(envVariable)) { 
                MessageBox.Show($@"User environment variable: 
Current:{Tab}'{EaScriptHomeEnvName}'={envVariable}
Expected:{Tab}'{EaScriptHomeEnvName}'={pathDirectory}
Equal:{Tab}{envVariable.ToLower() == pathDirectory.ToLower()}

deleted

Copy  with CTRL+C to Clipboard, ignore beep!", $"{GetScriptName()}: Home environment variable set");
            }
            else
            {
                MessageBox.Show($@"User environment variable: 
'{EaScriptHomeEnvName}'

not set", $"{GetScriptName()}: Home environment variable not set");
            }
            return true;
        }
        /// <summary>
        /// Set/Update the EA_SCRIPT_HOME environment variable with the current path
        /// </summary>
        /// <returns></returns>
        private static bool SetUserScriptHomeEnv()
        {
            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Environment.SetEnvironmentVariable(EaScriptHomeEnvName, pathDirectory, EnvironmentVariableTarget.User);
            MessageBox.Show($@"User environment variable: 
'{EaScriptHomeEnvName}'={pathDirectory}

set

Copy  with CTRL+C to Clipboard, ignore beep!", $"{GetScriptName()}: Home environment variable set");
            return true;
        }
        /// <summary>
        /// Delete the EA_SCRIPT_HOME environment variable with the current path
        /// </summary>
        /// <returns></returns>
        private static bool DelUserScriptHomeEnv()
        {
            Environment.SetEnvironmentVariable(EaScriptHomeEnvName, null, EnvironmentVariableTarget.User);
            MessageBox.Show($@"User environment variable: 
'{EaScriptHomeEnvName}'

deleted

Copy  with CTRL+C to Clipboard, ignore beep!", $"{GetScriptName()}: Home environment variable deleted");
            return true;
        }

        /// <summary>
        /// Get the *.exe name
        /// </summary>
        /// <returns></returns>
        private static string GetScriptName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }
        /// <summary>
        /// Get the *.exe full name
        /// </summary>
        /// <returns></returns>
        private static string GetScriptFullName()
        {
            return $"'{Assembly.GetExecutingAssembly().Location}'";
        }

    }
}
