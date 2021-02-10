using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;


namespace spb2xml
{
    public class ModelBank
    {
        private Hashtable models = new Hashtable();
    

        public ModelBank(string modelFileUrl)
        {
            StreamReader sr = new StreamReader(modelFileUrl);
            string line;
            char[] delimiters = new char[] { ' ', '\t' };
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    Guid g = new Guid(parts[1].Trim());
                    String name = parts[0].Trim();
                    if (!models.ContainsKey(g))
                    {
                        models.Add(g, name);
                    }
                }
            }
        }

        public string Lookup(Guid g)
        {
            return (string) models[g];
        }

        public int Size()
        {
            return models.Count;
        }
    }
}
