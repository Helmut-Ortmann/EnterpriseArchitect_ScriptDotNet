using System;
using System.Linq;
using System.Xml.Linq;

namespace ho.ScriptDotNet.CSharp
{
    /// <summary>
    /// XML Helper functions to convert SQL XML output to XML Search format
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Make EA XML output format from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <returns>string</returns>
        public static string MakeEaXmlOutput(string x)
        {
            if (string.IsNullOrEmpty(x)) return EmptyQueryResult();
            return MakeEaXmlOutput(XDocument.Parse(x));
        }

        /// <summary>
        /// Make EA XML output format from EA SQLQuery XDocument format (LINQ to XML). If nothing found or an error has occurred nothing is displayed.
        /// </summary>
        /// <param name="x">Output from EA SQLQuery</param>
        /// <returns></returns>
#pragma warning disable CSE0003 // Use expression-bodied members
        private static string MakeEaXmlOutput(XDocument x)
        {
            //---------------------------------------------------------------------
            // make the output format:
            // From Query:
            //<EADATA><Dataset_0>
            // <Data>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            // </Data>
            //</Dataset_0><EADATA>
            //
            //-----------------------------------
            // To output EA XML:
            //<ReportViewData>
            // <Fields>
            //   <Field name=""/>
            //   <Field name=""/>
            // </Fields>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            //</reportViewData>
            try
            {
                return new XDocument(
                    new XElement("ReportViewData",
                        new XElement("Fields",
                               from field in x.Descendants("Row").FirstOrDefault()?.Descendants()
                               select new XElement("Field", new XAttribute("name", field.Name))
                        ),
                        new XElement("Rows",
                                    from row in x.Descendants("Row")
                                    select new XElement(row.Name,
                                           from field in row.Nodes()
                                           select new XElement("Field", new XAttribute("name", ((XElement)field).Name),
                                                                        new XAttribute("value", ((XElement)field).Value)))

                    )
                )).ToString();
            }
            catch (Exception)
            {
                // empty query result
                return EmptyQueryResult();

            }
        }
        #region Empty Query Result
        /// <summary>
        /// Empty Query Result
        /// </summary>
        /// <returns></returns>
        private static string EmptyQueryResult()
        {
            return new XDocument(
                new XElement("ReportViewData",
                    new XElement("Fields",
                           new XElement("Field", new XAttribute("name", "Empty"))
                    ),
                    new XElement("Rows",
                        new XElement("Row",
                                new XElement("Field",
                                                    new XAttribute("name", "Empty"),
                                                    new XAttribute("value", "__empty___")))

                )
            )).ToString();
            #endregion
        }
    }
}
