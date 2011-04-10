#define TRACE
using System;
using System.Collections;
using System.Text;
using System.Diagnostics;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLogger(DateTime.Now);
            Trace.WriteLine("Starting Main method of Assembler", "Main");

            Parser pars = new Parser();
            IntermediateFile interSource;
            SymbolTable symb;
            pars.ParseSource(args[0], out interSource, out symb);

            Console.WriteLine(interSource);
            Console.WriteLine(symb);
        }

        /* Two's Complement stuff.
         *  short varA = 1023;
-            short varB = 367;
-            long result = (varA ^ varB) + 1;
-            Console.WriteLine(result);
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
