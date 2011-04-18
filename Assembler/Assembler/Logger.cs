using System;
using System.IO;
using System.Diagnostics;

namespace Assembler
{
    /**
     * Logger used throughout the assembler to create log reports which may be used for debugging should an error occur.
     */
    class Logger
    {
        private static bool NeedInit = true;
        /**
         * Initializes the Logger, making a Log directory if needed.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 6, 2011
         * @modlog
         *  - April  6, 2011 - Mark - Creates a log file.
         *  - April  7, 2011 - Mark - Creates Log directory if needed.
         *  - April 12, 2011 - Mark - Moved from the Program class to the Logger288 class.
         *  - April 18, 2011 - Andrew - Changed name from Logger288 to Logger. :(
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void InitLogger(DateTime now)
        {
            NeedInit = false;
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

            Logger.Log(String.Format("{0} {1}\n{2}\n", "Log started at", now,
                "----------------------------------------------------------------"));
        }

        public static void Log(string message)
        {
            Trace.WriteLine(message);
        }
        public static void Log(string message, string loc)
        {
            if (Logger.NeedInit)
            {
                Logger.InitLogger(DateTime.Now);
            }
            Trace.WriteLine(message, loc);
        }
    }
}
