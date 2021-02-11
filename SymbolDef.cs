using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace spb2xml
{
    [Serializable]
    public class SymbolDef : DefinitionElement
    {

        private string mVersion;

        public string version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }

        List<PropertyDef> properties = new List<PropertyDef>();
        List<SetDef> sets = new List<SetDef>();
        List<TypeDef> types = new List<TypeDef>();

        public SymbolDef(string dirName, XmlNode node)
            : base(node)
        {
            SymbolBank bank = SymbolBank.Instance;

            foreach (XmlNode son in node.ChildNodes) 
            {
                string sonName = son.Name;

                if (sonName.Equals("SymbolInclude"))
                {
                    string symbolFile = son.Attributes["filename"].Value;
                    bank.AddSymbolDefinitionFile(dirName + "\\" + symbolFile);
                }


                if (sonName.Equals("TypeDefs"))
                {
                    // parse types
                    foreach (XmlNode typeNode in son.ChildNodes)
                    {
                        if (typeNode.Name.Equals("TypeDef"))
                        {
                            TypeDef type = new TypeDef(typeNode);
                            bank.AddType(type);
                        }
                    }
                }

                if (sonName.Equals("PropertyDefs"))
                {
                    // parse properties
                    foreach (XmlNode propNode in son.ChildNodes)
                    {
                        if (propNode.Name.Equals("PropertyDef"))
                        {
                            PropertyDef prop = new PropertyDef(propNode);
                            prop.SymbolContext = this;
                            bank.AddProperty(prop);
                        }
                    }
                }

                if (sonName.Equals("SetDefs"))
                {
                    // parse types
                    foreach (XmlNode setNode in son.ChildNodes)
                    {
                        if (setNode.Name.Equals("SetDef"))
                        {
                             SetDef set = new SetDef(setNode);
                             set.Parent = this;
                             bank.AddSet(set);
                        }
                    }
                }

            }
        }
    }
}
