#define TRACE
using System;
using System.Diagnostics;

namespace Assembler
{
    class Program
    {
        /**
         * Drives the Assembler.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg
         *  - Incorrect number of arguments.
         *      - Expected 1 parameter, but received [actual args.Length]
         * @author Mark
         * @creation April 5, 2011
         * @modlog
         *  - April  6, 2011 - Mark - Initializes Logger.
         *  - April  8, 2011 - Mark - Reads files in from the command line.
         *  - April  8, 2011 - Mark - Initializes and uses a Parser object.
         *  - April  8, 2011 - Mark - Outputs results to screen.
         *  - April  9, 2011 - Mark - Uses an IntermediateFile object.
         *  - April  9, 2011 - Mark - Catches problem with improper number of arguments.
         *  - April 10, 2011 - Mark - Uses a SymbolTable object.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        static void Main(string[] args)
        {
            // initialize logger
            Logger288.Log("Starting Main method of Assembler", "Main");
            
            // this will hold the file names.
            string[] file = null;

            // check that the arguments are of the proper number
            if (args.Length >= 1)
            {
                Logger288.Log("Arguments are valid", "Main");
                file = args;
            }
            else
            {
                // bad. exit.
                string error = "Expected 1 parameter, but received " + args.Length;
                Logger288.Log(error, "Main");
                Console.Error.WriteLine(error);
                Console.Error.WriteLine("Program will now exit.");
                System.Environment.Exit(1);
            }

            // parse each file passed in through args.  doesn't actually keep parsed output yet.
            foreach (string path in file)
            {
                Parser pars = new Parser();
                IntermediateFile interSource;
                SymbolTable symb;
                pars.ParseSource(path, out interSource, out symb);

                Logger288.Log("Printing output to screen", "Main");
                Console.WriteLine(interSource);
                Console.WriteLine(symb);
            }
        }
    }
}
