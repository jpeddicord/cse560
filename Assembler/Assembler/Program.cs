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
            Trace.WriteLine(String.Format("{0} {1}", System.DateTime.Now, "Starting Main method of Assembler"), "Main");
			Console.WriteLine("Herp derp");
            Console.WriteLine(Properties.Resources.instructions);
            Parser immaParser = new Parser();
            Tokenizer immaTokenizer = new Tokenizer();
        }
    }
}
