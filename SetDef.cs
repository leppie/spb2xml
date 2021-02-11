using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace spb2xml
{
    [Serializable]
    public class SetDef : DefinitionElement
    {
        private List<string> mProperties = new List<string>();

        private SymbolDef mParent;

        public SymbolDef Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }
	

        public SetDef(XmlNode node)
            : base(node)
        {
            // get properties
            foreach (XmlNode son in node)
            {
                if (son.Name.Equals("property"))
                {
                    XmlAttribute sonPropName = son.Attributes["name"];
                    if (sonPropName != null)
                    {
                        mProperties.Add(sonPropName.Value);
                    }
                }
            }
        }
    }
}
