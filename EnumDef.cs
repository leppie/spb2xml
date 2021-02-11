using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace spb2xml
{
    [Serializable]
    public class EnumDef
    {
        private List<String> values = new List<string>();
       
        public EnumDef(XmlNode node)
        {
            foreach (XmlNode son in node.ChildNodes)
            {
                if (son.Name.Equals("EnumVal"))
                {
                    XmlAttributeCollection attributes = son.Attributes;
                    XmlAttribute attr;

                    string valName = null;
                    attr = attributes["name"];
                    if (attr != null) valName = attr.Value;
                    attr = attributes["xml_name"];
                    if (attr != null) valName = attr.Value;

                    if (valName != null) values.Add(valName);
                }
            }
        }

        public string this[int index]
        {
            get
            {
                if (index > values.Count) throw new SPBException("Enum value out of bound");
                return values[index];
            }
        }
    }
}
