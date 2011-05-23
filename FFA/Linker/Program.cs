using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Error = Assembler.ErrorException;

namespace Linker
{
    class Program
    {
        static void Main(string[] args)
        {
             Assembler.Errors.SetResource(Properties.Resources.Errors);

            string[] infiles = new string[1];
            if (args.Length > 0)
            {
                infiles = args;
            }

            string outfile = infiles[0] + ".ffa";

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
