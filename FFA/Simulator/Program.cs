using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    /**
     * Main startup program.
     */
    class Program
    {
        public static void Usage()
        {
            Console.WriteLine("FFA Simulator");
            Console.WriteLine("Usage: Simulator.exe [-d] program.ffa");
            Console.WriteLine("If -d is given, will run in debug mode.");
        }

        static void Main(string[] args)
        {
            Assembler.Errors.SetResource(Properties.Resources.errors);

            bool debug = false;
            string filename = "";
            if (args.Length == 1)
            {
                filename = args[0];
            }
            else if (args.Length == 2 && args[0] == "-d")
            {
                debug = true;
                filename = args[1];
            }
            else
            {
                Usage();
                System.Environment.Exit(1);
            }

            var parser = new Parser();
            bool success = parser.ParseFile(filename);

            if (success)
            {
                Runtime.Debug = debug;
                Runtime.GetInstance().Run();
            }
        }
    }
}
