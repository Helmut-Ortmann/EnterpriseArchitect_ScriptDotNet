using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils;

namespace ScriptsCSharpLinq.CSharpLinq
{
    /// <summary>
    /// Show LINQ for SQL access
    /// </summary>
    public class ScriptCSharpLinq
    {
        private readonly EA.Repository _repository;
        public readonly string Tab = "\t"; // Tabulator
        public readonly string EaOutputTabName = "EaScript";

        public ScriptCSharpLinq(int pid)
        {
            _repository = SparxSystems.Services.GetRepository(pid);
            Trace("Running C# Console Application 'ScriptDoNet.exe'");
        }

        /// <summary>
        /// LINQ for SQL
        /// Print the package name und select the package in browser.
        /// </summary>
        /// <param name="args[2]">optional: guid of the start package</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool LinqForSql(string[] args)
        {
            // get connection string of repository
            string connectionString = LinqUtil.GetConnectionString(_repository, out var provider);
            DataTable dt;
            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                dt = (from o in db.t_object
                        orderby new { o.Name, o.Object_Type, o.Stereotype }
                        select new { CLASSGUID = o.ea_guid, CLASSTYPE = o.Object_Type, Name = o.Name, Type = o.Object_Type, Stereotype = o.Stereotype }
                    ).Distinct()
                    .ToDataTable();

            }
            // 2. Order, Filter, Join, Format to XML
            string xml = LinqUtil.QueryAndMakeXmlFromTable(dt);
            // 3. Out put to EA
            _repository.RunModelSearch("", "", "", xml);
            return true;
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
