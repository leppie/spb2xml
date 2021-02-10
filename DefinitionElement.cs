using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace spb2xml
{
    public abstract class DefinitionElement
    {
        private string mName;
        private Guid mGUID;
        private string mDesc;
        private SymbolDef mParentSymbol;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public Guid ID
        {
            get
            {
                return mGUID;
            }
            set
            {
                mGUID = value;
            }
        }

        public string Description
        {
            get
            {
                return mDesc;
            }

            set
            {
                mDesc = value;
            }
        }

        public SymbolDef SymbolContext
        {
            get
            {
                return mParentSymbol;
            }

            set
            {
                mParentSymbol = value;
            }
        }


        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DefinitionElement de = obj as DefinitionElement;
            if (de == null)
            {
                return false;
            }

            return de.mGUID.Equals(mGUID) && de.mName.Equals(mName);
        }

        public override int GetHashCode()
        {
            return mName.GetHashCode() ^ mGUID.GetHashCode();
        }

        public DefinitionElement(XmlNode node)
        {
            XmlAttributeCollection attrs = node.Attributes;
            XmlAttribute attr;

            attr = attrs["name"];
            if (attr != null) Name = attr.Value;
            attr = attrs["Name"];
            if (attr != null) Name = attr.Value;
            attr = attrs["id"];
            if (attr != null) ID = new Guid(attr.Value);
            attr = attrs["ID"];
            if (attr != null) ID = new Guid(attr.Value);
            attr = attrs["Id"];
            if (attr != null) ID = new Guid(attr.Value);
            attr = attrs["descr"];
            if (attr != null) Description = attr.Value;
        }
    }
}
