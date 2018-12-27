using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ho.ScriptDotNet.CSharp;


namespace ho.ScriptDotnet.CSharp
{
    public class ScriptCSharp
    {
        private readonly EA.Repository _repository;
        public readonly string Tab = "\t"; // Tabulator
        public readonly string EaOutputTabName = "EaScript";

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
        /// <param name="args"></param>
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
        /// List all diagram elements.
        /// </summary>
        /// <param name="args[2]">guid of the diagram</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool ListDiagramElements(string[] args)
        {
            // guid passed, use the selected diagram
            if (args.Length > 2 && args[2].StartsWith("{"))
            {
                EA.Diagram dia = (EA.Diagram)_repository.GetDiagramByGuid(args[2].Trim());
                if (dia != null)
                {
                    foreach (EA.DiagramObject diaObj in dia.DiagramObjects)
                    {
                        EA.Element el = _repository.GetElementByID(diaObj.ElementID);
                        if (el != null)
                        {
                            Print(el.Name);
                            _repository.ShowInProjectView(el);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No Diagram guid passed");
                MessageBox.Show("Diagram guid should be passed", "No Diagram guid passed");
                return false;
            }
         

            return true;
        }
        /// <summary>
        /// EA shows ModelSearches in the Context menu of the found rows in the Search Window. The Search must contain 'CLASSGUID', CLASSTYPE' to identify the EA item.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool ModelSearch(string[] args)
        {
            // Get the context item which represents the selected row of the search results
            EA.ObjectType objType = _repository.GetContextItem(out object contextObject);
            EA.Element el;
            string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            switch (objType)
            {
                case EA.ObjectType.otElement:
                    el = (EA.Element) contextObject;
                    
                    Print($@"You have selected Element:");

                    Print($@"Name{delimiter}'{el.Name}'");
                    Print($@"Type{delimiter}'{el.Type}'");
                    Print($@"Stereotype{delimiter}'{el.Stereotype}'");
                    Print($@"StereotypeEx{delimiter}'{el.StereotypeEx}'");
                    _repository.ShowInProjectView(el);
                    break;

                case EA.ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)contextObject;
                    el = _repository.GetElementByGuid(pkg.PackageGUID);
                    Print($@"You have selected Package:");

                    Print($@"Name{ delimiter}'{pkg.Name}'");
                    Print($@"Type{delimiter}'{el.Type}'");
                    Print($@"Stereotype{delimiter}'{el.Stereotype}'");
                    Print($@"StereotypeEx{delimiter}'{el.StereotypeEx}'");
                    _repository.ShowInProjectView(pkg);
                    break;

            }
            return true;
        }
        /// <summary>
        /// EA shows ProjectSearches in the Context menu of the found rows in the Search Window. The Search must contain 'CLASSGUID', CLASSTYPE' to identify the EA item.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool ProjectSearch(string[] args)
        {
            // Get the context item 
            EA.ObjectType objType = _repository.GetContextItem(out object contextObject);
            switch (objType)
            {
                case EA.ObjectType.otElement:
                    EA.Element el = (EA.Element)contextObject;
                    break;
                case EA.ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)contextObject;
                    break;

            }

            string sql = @"select o.ea_guid as [CLASSGUID], o.object_type as [CLASSTYPE],
                            o.Name, o.object_type, o.Stereotype
                        from t_object o
                        order by 3,4,5";
            string xml = _repository.SQLQuery(sql);
            // output the query in EA Search Window format
            string xmlEaOutput = XmlHelper.MakeEaXmlOutput(xml);
            _repository.RunModelSearch("","", "", xmlEaOutput);
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
        public void Trace(string msg)
        {
            // Output message 
            Console.WriteLine(msg);

        }
        /// <summary>
        /// Output to EA System Output, Tab 'Script'
        /// </summary>
        /// <param name="msg">The message to output in EA</param>
        public void Print(string msg)
        {
            // Displays the message in the 'Script' tab of Enterprise Architect System Output Window
            _repository.CreateOutputTab(EaOutputTabName);
            _repository.EnsureOutputVisible(EaOutputTabName);
            _repository.WriteOutput(EaOutputTabName, msg, 0);

        }
    }
}
