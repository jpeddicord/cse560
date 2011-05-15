#define TRACE
using System;
using System.Diagnostics;

namespace Assembler
{
    /**
     * Drives the assembler.
     */
    public class Program
    {
        /**
         * Output usage information.
         */
        public static void Usage()
        {
            Console.WriteLine("FFA Assembler");
            Console.WriteLine("Usage:\t\tAssembler source [output]");
            Console.WriteLine("If no output file is given standard output will be used.");
        }

        /**
         * Drives the Assembler.
         *
         * @param args program runtime arguments
         *
         * @refcode N/A
         * @errtest
         *  Differing program arguments.
         * @errmsg
         *  - Incorrect number of arguments.
         *  - Usage information
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
         *  - May    8, 2011 - Jacob - Only process one file.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Main(string[] args)
        {
            string infile = null;
            string outfile = null;

            // single arg given, output to stdout
            if (args.Length == 1)
            {
                infile = args[0];
                outfile = args[0] + ".obj";
            }
            // two args given, read in the first and output to the second
            else if (args.Length == 2)
            {
                infile = args[0];
                outfile = args[1];
            }
            // invalid; print usage
            else
            {
                Usage();
                System.Environment.Exit(1);
            }

            // pass 1
            Logger.Log("Starting up", "Main");
            SymbolTable symb = new SymbolTable();
            Parser pars = new Parser();
            IntermediateFile interSource;
            pars.ParseSource(infile, out interSource, out symb);


            bool fatalerr = false;
            AssemblyReport report = new AssemblyReport();

            // Checking for fatal errors and creating an Assembly Report to display if a fatal error is found.
            foreach (IntermediateLine line in interSource)
            {
                if (line.Fatal)
                {
                    fatalerr = true;
                }

                report.Add(line.ProgramCounter, "0000", ' ', line.SourceLineNumber, line.SourceLine, line.GetThreeErrors());
            }

            if (fatalerr)
            {
                // Output the report
                Console.WriteLine(report);
                // and symbol table
                Console.WriteLine(symb);
                // Stop the assembler
                System.Environment.Exit(10);
            }

#if DEBUG
            Console.WriteLine(interSource);
#endif

            // pass 2
            Logger.Log("Starting pass 2", "Main");
            interSource.CalculateModuleLength();
            report = new AssemblyReport();
            ObjectFile obj = new ObjectFile(ref interSource, ref symb, ref report);
            obj.Render(outfile);

            // print out assembly report
            Console.WriteLine(report);
            // and symbol table
            Console.WriteLine(symb);
        }
    }
}
