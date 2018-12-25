using System;


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
        /// Traverse package. If a package guid is passed start from this package. If not show all model packages.
        /// Print the package name und select the package in browser.
        /// </summary>
        /// <param name="args[2]">optional: guid of the start package</param>
        /// <returns></returns>
        public bool TraversePackage(string[] args)
        {
            EA.Collection packages = _repository.Models;
            // guid passed, use the selected package as start package
            if (args.Length > 2 && args[2].StartsWith("{"))
            {
                var startPackage = _repository.GetPackageByGuid(args[2].Trim());
                if (startPackage != null)
                {
                    PrintPackageRecursive(startPackage);
                    return true;
                }
            }
            
            foreach (EA.Package pkg in packages)
            {
                PrintPackageRecursive(pkg);
            }

            return true;
        }
        /// <summary>
        /// Print all packages and locate each package
        /// </summary>
        /// <param name="package"></param>
        private void PrintPackageRecursive(EA.Package package)
        {
            Trace(package.Name);
            _repository.ShowInProjectView(package);
            Print(package.Name);
            EA.Collection packages = package.Packages;
            foreach (EA.Package pkg in packages)
            {
                 PrintPackageRecursive(pkg);
            }
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
