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
            Console.WriteLine("Usage:\t\tLinker obj1.obj [obj2.obj...]");
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
                Usage();
                System.Environment.Exit(1);
            }

            string outfile = System.IO.Path.GetFileNameWithoutExtension(infiles[0]) + ".ffa";

            Parser pars = new Parser();

            int file = 0;
            int address = 0;
            SymbolTable symb = new SymbolTable();
            Module[] mods = new Module[infiles.Length];

            try
            {
                foreach (var f in infiles)
                {
                    Module mod;
                    pars.ParseFile(f, out mod, symb, file, ref address);
                    mods[file] = mod;
                    file++;
                }

                LoadFile load = new LoadFile(mods, symb);
                load.Render(outfile);
            }
            catch (Error)
            {
                // gracefully die on fatal exceptions
                Console.WriteLine("Stopping linker due to fatal error.");
                System.Environment.Exit(1);
            }
        }
    }
}
