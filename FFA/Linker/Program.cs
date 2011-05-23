using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Error = Assembler.ErrorException;

namespace Linker
{
    class Program
    {
        /**
         * Output usage information.
         */
        public static void Usage()
        {
            Console.WriteLine("FFA Linker");
            Console.WriteLine("Usage:  Linker obj1.obj [obj2.obj...]");
            Console.WriteLine("Linker will link all obj files into obj1.ffa");
        }

        static void Main(string[] args)
        {
             Assembler.Errors.SetResource(Properties.Resources.Errors);

            string[] infiles = new string[1];
            if (args.Length > 0)
            {
                infiles = args;
            }
            else
            {
                // there must be at least one input file
                Usage();
                System.Environment.Exit(1);
            }

            string outfile = System.IO.Path.GetFileNameWithoutExtension(infiles[0]) + ".ffa";

            Parser pars = new Parser();

            int file = 0;
            int address = 0;
            SymbolTable symb = new SymbolTable();
            List<Module> modules = new List<Module>();
            Module[] mods = new Module[infiles.Length];

            try
            {
                foreach (var f in infiles)
                {
                    Module mod = new Module();
                    try
                    {
                        pars.ParseFile(f, out mod, symb, file, ref address);
                    }
                    catch (Error ex)
                    {
                        if (ex.err.category == Assembler.Errors.Category.Fatal)
                        {
                            throw ex;
                        }
                        else
                        {
                            Console.WriteLine(ex.err);
                            continue;
                        }
                    }
                    modules.Add(mod);
                    mods[file] = mod;
                    file++;
                }

                if (modules.Count > 0)
                {
                    LoadFile load = new LoadFile(modules, symb);
                    load.Render(outfile);
                }
                else
                {
                    Assembler.Errors.GetInstance().PrintError(Assembler.Errors.Category.Serious, 57);
                }
                Console.WriteLine(symb);
            }
            catch (Error)
            {
                // gracefully die on fatal exceptions
                Console.WriteLine("Stopping linker due to fatal error.");
                Console.WriteLine(symb);
                System.Environment.Exit(1);                
            }           
        }
    }
}
