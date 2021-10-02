

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace spb2xml
{
    /// <summary>
    /// singleton class containing all symbols and property definitions
    /// </summary>
    [Serializable]
    public sealed class SymbolBank
    {
        private Hashtable guidMap = new Hashtable();
        private Hashtable typesMap = new Hashtable();
        private List<SymbolDef> symbolDefs = new List<SymbolDef>();
        private List<string> symbolDefsNames = new List<string>();
 
        /// <summary>
        /// Initialize a blank symbol bank
        /// </summary>
        private SymbolBank()
        {
        }

        public bool AddSymbolDefinitionFile(string url)
        {
            // extract directory
            string dirName = new FileInfo(url).Directory.Parent.FullName;

            XmlDocument doc = new XmlDocument();
            doc.Load(url);

            foreach (XmlNode n in doc.GetElementsByTagName("SymbolDef")) {
                // pre-read name
                XmlAttribute attr = n.Attributes["name"];
                if (attr != null && !symbolDefsNames.Contains(attr.Value))
                {
                    SymbolDef sd = new SymbolDef(dirName, n);
                    symbolDefsNames.Add(attr.Value);
                    symbolDefs.Add(sd);
                }
            }

            return true;
        }

        private static SymbolBank instance = null;
        private static readonly object locker = new object();

        public static SymbolBank Instance
        {

            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new SymbolBank();
                    }
                    return instance;
                }
            }
            set
            {
                instance = value;
            }
        }

        public void AddType(TypeDef type) 
        {
            AddAndCheck(type, typesMap);
        }

        public void AddProperty(PropertyDef prop)
        {
            AddAndCheck(prop);
        }

        public void AddSet(SetDef set)
        {
            AddAndCheck(set);
        }

        private void AddAndCheck(DefinitionElement element, Hashtable col)
        {
            string eltName = element.Name.ToLower();
            if (col.ContainsKey(eltName) || guidMap.ContainsKey(element.ID))
            {
                Debug.WriteLine("Addding " + element.Name + " id " + element.ID + " already exists");
                return;
            }
            col.Add(eltName, element);
            guidMap.Add(element.ID, element);
        }

        private void AddAndCheck(DefinitionElement element)
        {
            if (guidMap.ContainsKey(element.ID))
            {
                Debug.WriteLine("Addding " + element.Name + " id " + element.ID + " already exists");
                return;
            }
            guidMap.Add(element.ID, element);
        }

        public TypeDef LookupType(string typeName)
        {
            return (TypeDef) typesMap[typeName.ToLower()];
        }

        public DefinitionElement LookupElement(Guid g)
        {
            return (DefinitionElement) guidMap[g];
        }
    }
}
