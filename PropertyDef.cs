using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace spb2xml
{
    public class PropertyDef : DefinitionElement
    {
        private bool mIsAttribute = false;

        public bool Attribute
        {
            get { return mIsAttribute; }
            set { mIsAttribute = value; }
        }

        private TypeDef mType;

        public TypeDef Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public bool IsAttribute()
        {
            return mIsAttribute;
        }

        private EnumDef mEnum;

        public EnumDef Enum
        {
            get
            {
                return mEnum;
            }
        }

        public PropertyDef(XmlNode node)
            : base(node)
        {
            SymbolBank bank = SymbolBank.Instance;

            // just the attribute
            XmlAttribute attr = node.Attributes["xml_io"];
            if (attr != null)
            {
                if ("attribute".Equals(attr.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    mIsAttribute = true;
                }
            }

            attr = node.Attributes["type"];
            if (attr != null) Type = bank.LookupType(attr.Value);

            attr = node.Attributes["Type"];
            if (attr != null) Type = bank.LookupType(attr.Value);

            foreach (XmlNode son in node.ChildNodes)
            {
                if (son.Name.Equals("EnumDef"))
                {
                    mEnum = new EnumDef(son);
                    // set type to enu
                }
            }
        }
    }
}
