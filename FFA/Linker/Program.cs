using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            string outfile = infiles[0] + ".FFA";

            Parser pars = new Parser();

            int file = 0;
            int address = 0;
            SymbolTable symb = new SymbolTable();
            Dictionary<string, Module> modules = new Dictionary<string, Module>();
            Module[] mods = new Module[infiles.Length];

            foreach (var f in infiles)
            {
                Module mod;
                pars.ParseFile(f, out mod, symb, file, ref address);
                modules.Add(mod.ModuleName, mod);
                mods[file] = mod;
                file++;
            }

            LoadFile load = new LoadFile(mods, symb);
            load.Render(outfile);
        }
    }
}
