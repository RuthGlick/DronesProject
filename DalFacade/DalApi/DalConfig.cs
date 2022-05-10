using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DalApi
{
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, (string, string, string)> DalPackages;

        /// <summary>
        /// read from configuration file which object which realize IDal to create.
        /// </summary>
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"../../../../DalXml\xml\dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal-packages").Elements()
                           select pkg
                          ).ToDictionary(p => "" + p.Name, 
                          p => (NamespaceName: p.Attribute("namespace").Value, ClassName: p.Attribute("class").Value, AssemblyName: p.Value));
        }
    }
}
