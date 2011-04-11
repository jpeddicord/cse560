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
            InitLogger(DateTime.Now);
            Trace.WriteLine("Starting Main method of Assembler", "Main");


            string[] file = null;

            // check that the arguments are of the proper number
            if (args.Length >= 1)
            {
                Trace.WriteLine("Arguments are valid", "Main");
                file = args;
            }
            else
            {
                // bad. exit.
                string error = "Expected 1 parameter, but received " + args.Length;
                Trace.WriteLine(error, "Main");
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

                Console.WriteLine(interSource);
                Console.WriteLine(symb);
            }
        }

        /**
         * Initializes the Logger, making a Log directory if needed.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 6, 2011
         * @modlog
         *  - April 6, 2011 - Mark - Creates a log file.
         *  - April 7, 2011 - Mark - Creates Log directory if needed.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        static void InitLogger(DateTime now)
        {
            // Create the log directory if it doesn't already exist.
            if (!System.IO.Directory.Exists("Log"))
            {
                
                System.IO.Directory.CreateDirectory("Log");
            }

            // Creates the text file that the trace listener will write to.
            // log filenames will be in the format:
            // MM-DD_HH-mm-ss_debug.log
            System.IO.FileStream myTraceLog = new
               System.IO.FileStream(String.Format(
                                        "{0}{1}-{2}_{3}-{4}-{5}_{6}",
                                        "Log" + System.IO.Path.DirectorySeparatorChar,
                                        now.Month,
                                        now.Day,
                                        now.Hour,
                                        now.Minute,
                                        now.Second,
                                        "debug.log"),
               System.IO.FileMode.OpenOrCreate);
            // Creates the new trace listener.
            TextWriterTraceListener myListener = new TextWriterTraceListener(myTraceLog);
            Trace.Listeners.Add(myListener);
            Trace.AutoFlush = true;

            Trace.WriteLine(String.Format("{0} {1}\n{2}\n", "Log started at", now,
                "----------------------------------------------------------------"));
        }
    }
}
