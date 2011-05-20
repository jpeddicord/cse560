using System;
using System.IO;
using ErrCat = Assembler.Errors.Category;

namespace Simulator
{
    public class Parser
    {
        private string programName = "";
        private string loadAddress = "";
        private string executionStart = "";
        private int totalLength = 0;
        private int totalRecords = 0;

        public Parser()
        {

        }

        public void ParseFile(string filename)
        {
            var file = new StreamReader(filename);

            string line;
            int lineNum = 1;
            bool reachedEnd = false;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    if (line.Length > 0)
                    {
                        // don't allow garbage after the end
                        if (reachedEnd)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 9);
                        }

                        // get the parts of the string
                        string[] parts = line.Split(':');
    
                        // header record found
                        if (parts[0] == "H")
                        {
                            ParseHeader(parts);
                        }
                        // text record found
                        else if (parts[0] == "T")
                        {
                            ParseText(parts);
                        }
                        // end record found
                        else if (parts[0] == "E")
                        {
                            ParseEnd(parts);
                        }
                        else
                        {
                            // unknown record
                            throw new Assembler.ErrorException(ErrCat.Serious, 6);
                        }
                    }
                }
                catch (Assembler.ErrorException ex)
                {
                    Console.WriteLine(String.Format(
                            "Parsing error on line {0}:\n{1}",
                            lineNum, ex));
                    if (ex.err.category == ErrCat.Fatal)
                    {
                        Console.WriteLine("Aborting due to fatal errors.");
                        break;
                    }
                }

                lineNum++;
            }

            file.Close();
        }

        public void ParseHeader(string[] parts)
        {
            if (parts.Length != 13)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 1);
            }
            for (int i = 0; i < 13; i++)
            {

            }
        }

        public void ParseText(string[] parts)
        {
            // check the overall record length
            if (parts.Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 2);
            }
            // validate the memory address
            if (parts[1].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 7);
            }
            // validate the word
            if (parts[2].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 8);
            }
            // validate the program name
            if (parts[3] != this.programName)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 4);
            }

            // set the memory
            var mem = Memory.GetInstance();
            mem.SetWord(Convert.ToInt32(parts[1], 16),
                        Convert.ToInt32(parts[2], 16));
        }

        public void ParseEnd(string[] parts)
        {
            if (parts.Length != 2)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 3);
            }
            if (parts[1] != this.programName)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 4);
            }
        }
    }
}

