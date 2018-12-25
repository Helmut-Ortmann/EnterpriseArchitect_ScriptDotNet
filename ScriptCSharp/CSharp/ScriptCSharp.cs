using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ho.ScriptDotnet.CSharp
{
    public class ScriptCSharp
    {
        private readonly EA.Repository _repository;

        public ScriptCSharp(int pid)
        {
            _repository = SparxSystems.Services.GetRepository(pid);
            Trace("Running C# Console Application 'ScriptDoNet.exe'");
        }

        /// <summary>
        /// Traverse package. Print the package name und select the package in browser
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool TraversePackage(string[] args)
        {
            EA.Collection packages = _repository.Models;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                _repository.ShowInProjectView(child);
                Print(child.Name);
            }

            return true;
        }

        /// <summary>
        /// Trace to stdout which the calling EA Script receives
        /// </summary>
        /// <param name="msg">The message to output in EA</param>
        private void Trace(string msg)
        {
            // Output message 
            Console.WriteLine(msg);

        }
        /// <summary>
        /// Output to EA System Output, Tab 'Script'
        /// </summary>
        /// <param name="msg">The message to output in EA</param>
        private void Print(string msg)
        {
            // Displays the message in the 'Script' tab of Enterprise Architect System Output Window
            _repository?.WriteOutput("Script", msg, 0);

        }
    }
}
