#define TRACE
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Herp derp");
            Console.WriteLine(Properties.Resources.directives);
            Trace.WriteLine(String.Format("{0} {1}", System.DateTime.Now, "Herp"),"Derp");
            HERPDERPDERP.herp();
        }
    }
}
