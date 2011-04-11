#define TRACE
using System;
using System.Diagnostics;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLogger(DateTime.Now);
            Trace.WriteLine("Starting Main method of Assembler", "Main");


            string[] file = null;

            if (args.Length >= 1)
            {
                Trace.WriteLine("Arguments are valid", "Main");
                file = args;
            }
            else
            {
                string error = "Expected 1 parameter, but received " + args.Length;
                Trace.WriteLine(error, "Main");
                Console.Error.WriteLine(error);
                Console.Error.WriteLine("Program will now exit.");
                System.Environment.Exit(1);
            }

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
