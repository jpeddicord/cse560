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
            InitLogger(DateTime.Now);
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Starting Main method of Assembler"), "Main");
			Console.WriteLine("Herp derp");
            Console.WriteLine(Properties.Resources.instructions);
            Parser immaParser = new Parser();
            Tokenizer immaTokenizer = new Tokenizer();
        }

        static void InitLogger(DateTime now)
        {
            // Creates the text file that the trace listener will write to.
            System.IO.FileStream myTraceLog = new
               System.IO.FileStream(String.Format(
                                        "{0}{1}-{2}_{3}-{4}-{5}_{6}", 
                                        "Log\\",
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
