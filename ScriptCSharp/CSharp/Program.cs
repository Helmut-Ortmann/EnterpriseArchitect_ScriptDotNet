using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ho.ScriptDotnet.CSharp
{
    class Program
    {
        private static readonly string EaScripthomeEnvName = "EA_SCRIPT_HOME";
        private static readonly string Tab = "\t";
        /// <summary>
        /// Entry of the ScriptCSharp.exe to be called from EA Script (JScript, JavaScript, VB Script)
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
            if (HandleAdminRequest(args)) return;
            ScriptCSharp scriptCSharp = GetScriptCSharp(args);
            string command = GetCommand(args);
            if (scriptCSharp != null && !String.IsNullOrWhiteSpace(command))
            {
                bool returnValue = false;
                switch (command)
                {
                    case "TraversePackage":
                        returnValue = scriptCSharp.TraversePackage(args);
                        break;
                    case "SetEnvHome":
                        SetUserScriptHomeEnv();
                        break;
                    case "DelEnvHome":
                        DelUserScriptHomeEnv();
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
        static ScriptCSharp GetScriptCSharp(string[] args)
        {
            if (args.Length > 0)
            {
                if (!Int32.TryParse(args[0], out int pid))
                {
                    MessageBox.Show($"pid: {args[0]}", "Can't read pid (process id) from the first parameter");
                    return null;
                }

                return new ScriptCSharp(pid);
            }
            else
            {
                MessageBox.Show($"No first parameter available. Process ID required", "Can't read pid (process id) from the first parameter");
                return null;

            }

        }
        /// <summary>
        /// Gets the command from args
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Commad or "" if no command available</returns>
        static string GetCommand(string[] args)
        {
            if (args.Length > 1) return args[1];
            MessageBox.Show($"No second parameter with 'command' available. ", "Can't read the command to execute");
            return "";
        }
        /// <summary>
        /// Return error
        /// </summary>
        /// <param name="msg"></param>
        private static void ReturnError(string msg = "")
        {
            Console.WriteLine("Error");
        }
        /// <summary>
        /// Return ok
        /// </summary>
        /// <param name="msg"></param>
        private static void ReturnOk(string msg = "")
        {
            Console.WriteLine("Ok");
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
                    case "setenv":
                        SetUserScriptHomeEnv();
                        return true;

                    case "delenv":
                        DelUserScriptHomeEnv();
                        return true;

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
        /// Get info of ScriptCSharp
        /// </summary>
        private static bool GetInfo()
        {
            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envVariable = Environment.GetEnvironmentVariable(EaScripthomeEnvName, EnvironmentVariableTarget.User)??"";

            MessageBox.Show($@"Version:{Tab}'{Assembly.GetEntryAssembly().GetName().Version}'
Path:{Tab}{GetScriptFullName()}
Current Env:{Tab}'{EaScripthomeEnvName}'={envVariable}
Expected Env:{Tab}'{EaScripthomeEnvName}'={pathDirectory}
Equal Env:{Tab}{envVariable.ToLower() == pathDirectory.ToLower()}

Admin Commands:
'Info'{Tab}show this information
'GetEnv'{Tab}Get the '{EaScripthomeEnvName}' Env Variable
'SetEnv'{Tab}Set the '{EaScripthomeEnvName}' Env Variable with the own directory
'DelEnv'{Tab}Del the '{EaScripthomeEnvName}' Env Variable

Copy  with CTRL+C to Clipboard, ignore beep!
", "ScriptCSharp: Info");
            return true;
        }

        /// <summary>
        /// Get the EA_SCRIPT_HOME environment variable with the current path
        /// </summary>
        /// <returns></returns>
        private static bool GetUserScriptHomeEnv()
        {
            var pathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envVariable = Environment.GetEnvironmentVariable(EaScripthomeEnvName,  EnvironmentVariableTarget.User)??"";

            if (envVariable != null) { 
                MessageBox.Show($@"User environment variable: 
Current:{Tab}'{EaScripthomeEnvName}'={envVariable}
Expected:{Tab}'{EaScripthomeEnvName}'={pathDirectory}
Equal:{Tab}{envVariable.ToLower() == pathDirectory.ToLower()}

deleted

Copy  with CTRL+C to Clipboard, ignore beep!", $"{GetScriptName()}: Home environment variable set");
            }
            else
            {
                MessageBox.Show($@"User environment variable: 
'{EaScripthomeEnvName}'

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
            Environment.SetEnvironmentVariable(EaScripthomeEnvName, pathDirectory, EnvironmentVariableTarget.User);
            MessageBox.Show($@"User environment variable: 
'{EaScripthomeEnvName}'={pathDirectory}

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
            Environment.SetEnvironmentVariable(EaScripthomeEnvName, null, EnvironmentVariableTarget.User);
            MessageBox.Show($@"User environment variable: 
'{EaScripthomeEnvName}'

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
