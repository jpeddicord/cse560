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

            Console.WriteLine("herp");
            Parser pars = new Parser();

            int file = 0;
            int address = 0;
            SymbolTable symb = new SymbolTable();

            foreach (var f in infiles)
            {
                pars.ParseFile(f, file, ref address);
                file++;
            }
            
        }
    }
}
