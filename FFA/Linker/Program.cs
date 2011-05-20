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
            string[] infiles = new string[1];
            if (args.Length > 0)
            {
                infiles = args;
            }

            Console.WriteLine("herp");
            Parser pars = new Parser();

            foreach (var f in infiles)
            {
                pars.ParseFile(f);
            }
            
        }
    }
}
