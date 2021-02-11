
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace spb2xml

{
    class Program
    {

        public static void PrintHelp()
        {
            Console.WriteLine("Usage: spb2xml [-hv] [-s symboldir] [-m mdllist] [file.spb] [output.xml]");
            Console.WriteLine("\t-h\tPrint help");
            Console.WriteLine("\t-s\tSpecify simprop symbols search dir (Packages\\fs-base-propdefs\\Propdefs\\1.0\\)");
            //Console.WriteLine("\t-m\tSpecify path of a model list (same format as Autogen SDK\\library_objects.txt");
        }

        static void Main(string[] args)
        {
            string simPropSearchPath = null;
            string file = null;
            string outFileName = null;
            string modelsDescName = null;
            bool verbose = false;

            // big command line loop 
            for (int i = 0; i < args.Length; i++)
            {
                string s = args[i];
                if ("-h".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintHelp();
                    return;
                }
                else if ("-s".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (i == (args.Length - 1))
                    {
                        Console.WriteLine("Error: must specify simprop search path");
                        return;
                    }
                    simPropSearchPath = args[++i];
                }
                else if ("-m".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (i == (args.Length - 1))
                    {
                        Console.WriteLine("Error: must library models file path");
                        return;
                    }
                    modelsDescName = args[++i];
                }
                else if ("-v".Equals(s))
                {
                    verbose = true;
                }
                else if (file == null)
                {
                    file = s;
                }
                else if (outFileName == null)
                {
                    outFileName = s;
                }
                else
                {
                    Console.WriteLine("Error: unexpected argument {0}", s);
                    return;
                }
            }

            //
            // init simprop data
            //

            var cacheFn = "propdefs.cache";
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var cachePath = Path.Combine(exeDir, cacheFn);

            if (File.Exists(cachePath))
            {
                Console.WriteLine("Search property definition files from cache");
                using (var f = File.OpenRead(cachePath))
                {
                    var sb = (SymbolBank)new BinaryFormatter().Deserialize(f);
                    SymbolBank.Instance = sb;
                }
            }
            else
            {
                if (simPropSearchPath is null)
                {
#if DEBUG
                    simPropSearchPath = @"g:\MSFS Base\Packages\fs-base-propdefs\Propdefs\1.0\";
#else
                    Console.WriteLine("Error: FSX not found, you should specify simprop search path with -s");
                    return;
#endif
                }

                Console.WriteLine("Search property definition files in {0}", simPropSearchPath);

                //
                // parse all propdefs
                //
                SymbolBank sb = SymbolBank.Instance;
                DirectoryInfo cdi = new DirectoryInfo(simPropSearchPath);
                foreach (FileInfo fi in cdi.GetFiles("*.xml"))
                {
                    if (verbose)
                    {
                        Console.WriteLine("Add property definition file {0}", fi.Name);
                    }
                    try
                    {
                        sb.AddSymbolDefinitionFile(fi.FullName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Warning: Cannot parse property definition file {0}", fi.FullName);
                        if (verbose) Console.WriteLine("\t {0}", ex.Message);
                    }
                }

                using (var f = File.Create(cachePath))
                {
                    new BinaryFormatter().Serialize(f, SymbolBank.Instance);
                }
            }

            Console.WriteLine();

            //
            // force culture to us (better printing)
            // 
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            //
            // models bank
            // 
            ModelBank mb = null;
            if (modelsDescName != null)
            {
                try
                {
                    mb = new ModelBank(modelsDescName);
                    if (verbose)
                    {
                        Console.WriteLine("Read {0} models descriptions", mb.Size());
                    }
                }
                catch (Exception e)
                {
                    if (verbose)
                    {
                        Console.WriteLine("Cannot read models bank file: {0}", e.Message);
                    }
                }
            }

            if (file is null)
            {
                foreach (var f in Directory.GetFiles(".", "*.spb", SearchOption.AllDirectories))
                {
                    Decompile(f, null);
                }
            }
            else
            {
                Decompile(file, outFileName);
            }
            

            void Decompile(string f, string outFN)
            {
                //
                // ok for the real thing
                //
                try
                {
                    if (outFN == null)
                    {
                        // guess it from name
                        int i = f.LastIndexOf('.');
                        if (i != -1) outFN = f.Substring(0, i) + ".xml";
                        else outFN = f + ".xml";
                    }

                    using (Stream output = new FileStream(outFN, FileMode.Create))
                    {
                        Decompiler dec = new Decompiler(f);
                        if (mb != null) dec.SetModels(mb);
                        dec.Decompile(output);
                    }

                    if (outFN != null)
                    {
                        Console.WriteLine("Wrote to {0}", outFN);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: cannot decompile file {0} ({1})", f, e.Message);
                }
            }
        }
    }
}
