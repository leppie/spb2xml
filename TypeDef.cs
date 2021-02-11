using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace spb2xml
{
    [Serializable]
    public class TypeDef : DefinitionElement
    {
        private List<BindingMember> mBindingMembers = new List<BindingMember>();
        private string mBindingType;

        public string Type
        {
            get { return mBindingType; }
            set { mBindingType = value; }
        }

        public bool isComplexType()
        {
            return ((mBindingType != null) &&
                ("union".Equals(mBindingType) || "struct".Equals(mBindingType)));
        }


        public TypeDef(XmlNode node)
            : base(node)
        {
            // to resolve
            SymbolBank bank = SymbolBank.Instance;

            // find binding
            foreach (XmlNode son in node.ChildNodes) 
            {
                if (son.Name.Equals("binding"))
                {
                    foreach (XmlNode bindingSon in son.ChildNodes) 
                    {
                        if (bindingSon.Name.Equals("member")) 
                        {
                            XmlAttributeCollection attrs = bindingSon.Attributes;
                            string memberTypeName = attrs["type"].Value;
                            string memberName = attrs["name"].Value;
                            if (memberTypeName != null)
                            {
                                TypeDef memberType = bank.LookupType(memberTypeName);
                                BindingMember member = new BindingMember(memberName, null);
                            }
                        }
                    }

                    // get binding type
                    mBindingType = son.Attributes["type"].Value;
                }
            }
        }

    }

    [Serializable]
    public class BindingMember
    {
        private String mName;

        public String Name
        {
            get { return mName; }
        }

        private TypeDef mType;

        public TypeDef Type
        {
            get { return mType; }
        }

        public BindingMember(string name, TypeDef type)
        {
            mName = name;
            mType = type;
        }

    }


}
