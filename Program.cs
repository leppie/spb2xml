
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace spb2xml

{
    class Program
    {

        public static void PrintHelp()
        {
            Console.WriteLine("Usage: spb2xml [-hv] [-s symboldir] [-m mdllist] file.spb [output.xml]");
            Console.WriteLine("\t-h\tPrint help");
            Console.WriteLine("\t-s\tSpecify simprop symbols search dir (Packages\\fs-base-propdefs\\Propdefs\\1.0\\)");
            Console.WriteLine("\t-m\tSpecify path of a model list (same format as Autogen SDK\\library_objects.txt");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            string simPropSearchPath = null;
            string inFileName = null;
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
                else if (inFileName == null)
                {
                    inFileName = s;
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

            if (inFileName == null)
            {
                Console.WriteLine("Error: no file specified");
                return;
            }

            //
            // init simprop data
            //

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

            //
            // ok for the real thing
            //
            try
            {
                Stream output;
                if (outFileName == null)
                {
                    // guess it from name
                    int i = inFileName.LastIndexOf('.');
                    if (i != -1) outFileName = inFileName.Substring(0, i) + ".xml";
                    else outFileName = inFileName + ".xml";
                }
                output = new FileStream(outFileName, FileMode.Create);
                Decompiler dec = new Decompiler(inFileName);
                if (mb != null) dec.SetModels(mb);
                dec.Decompile(output);

                if (outFileName != null)
                {
                    Console.WriteLine("Wrote to {0}", outFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: cannot decompile file {0} ({1})", inFileName, e.Message);
            }
        }
    }
}
