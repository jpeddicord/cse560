using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    class LoadFile
    {
        Assembler.Errors errPrinter = Assembler.Errors.GetInstance();

        private Module[] modules;
        private SymbolTable symb;
        private Header loadHeader = new Header();

        public LoadFile(Module[] modules, SymbolTable symb)
        {
            this.modules = modules;
            this.symb = symb;
        }

        private void doModifyRecords(Module mod, SymbolTable symb)
        {
            var mRec = mod.ModifyRecords;
            foreach (var rec in mRec)
            {
                int word = mRec[rec.Key].Word;
                int s = Convert.ToInt32(Convert.ToString(word, 2).Substring(6), 2);
                List<string> adjs = mRec[rec.Key].Adjustments;

                for (int i = 0; i < adjs.Count; i += 2)
                {
                    string sign = adjs[i];
                    string label = adjs[i + 1];

                    try
                    {
                        if (sign == "+")
                        {
                            word += symb.GetSymbol(label).LinkerValue;
                            s += symb.GetSymbol(label).LinkerValue;
                        }
                        else if (sign == "-")
                        {
                            word -= symb.GetSymbol(label).LinkerValue;
                            s -= symb.GetSymbol(label).LinkerValue;
                        }
                        else
                        {
                            // error, the sign should only be a + or -
                            errPrinter.PrintError(ErrCat.Serious, 50);
                            continue;
                        }
                    }
                    catch (Assembler.SymbolException)
                    {
                        // error, symbol doesn't exist in symbol table
                        errPrinter.PrintError(ErrCat.Serious, 51);
                        continue;
                    }
                }

                mod.GetTextRecord(mRec[rec.Key].Location).Word = word;
            }
        }

        private void WriteTextRecords(StreamWriter writer)
        {
            foreach (var mod in modules)
            {
                var text = mod.TextRecords;

                foreach (var t in text)
                {
                    Text rec = text[t.Key];
                    rec.ProgramName = modules[0].HeaderRecord.ProgramName;
                    writer.WriteLine(rec.ToString());
                }
            }
        }

        private void WriteTextRecords()
        {
            foreach (var mod in modules)
            {
                var text = mod.TextRecords;

                foreach (var t in text)
                {
                    Text rec = text[t.Key];
                    rec.ProgramName = modules[0].HeaderRecord.ProgramName;
                    Console.WriteLine(rec.ToString());
                }
            }
        }

        public void Render(string filename)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filename);
            }
            catch (Exception)
            {
                errPrinter.PrintError(ErrCat.Serious, 52);
            }

            foreach (var mod in modules)
            {
                doModifyRecords(mod, symb);
            }

            // make the header record
            int totalLength = 0;
            int totalTextRecords = 0;
            foreach (var m in modules)
            {
                totalLength += m.HeaderRecord.ModuleLength;
                totalTextRecords += m.HeaderRecord.TotalTextRecords;
            }

            if (totalLength != totalTextRecords)
            {
                // error, should have the same number of text records as
                // the length of the program
                errPrinter.PrintError(ErrCat.Serious, 53);
            }

            loadHeader.ProgramName = modules[0].HeaderRecord.ProgramName;
            loadHeader.LinkerLoadAddress = modules[0].HeaderRecord.LinkerLoadAddress;
            loadHeader.ExecutionStartAddress = loadHeader.LinkerLoadAddress;
            loadHeader.ModuleLength = totalLength;
            loadHeader.TotalRecords = totalTextRecords + 2;
            loadHeader.TotalTextRecords = totalTextRecords;

            // create the end record of this object file
            string endRecord = String.Format("E:{0}", modules[0].HeaderRecord.ProgramName);

            // if the streamwriter was successfully instantiated
            // write the object file to a file on the disk
            if (writer != null)
            {
                try
                {
                    // write header record to file
                    writer.WriteLine(loadHeader.ToString());

                    // write the text records
                    WriteTextRecords(writer);

                    // write the end record
                    writer.WriteLine(endRecord);

                    // close the file
                    writer.Close();
                }
                catch (Exception)
                {
                    errPrinter.PrintError(ErrCat.Serious, 54);
                }
                finally 
                {
                    writer.Close();
                }
            }
            else
            {
                // something went wrong with the streamwriter
                // display the object file to the screen instead
                Console.WriteLine(loadHeader.ToString());

                WriteTextRecords();

                Console.WriteLine(endRecord);
            }
        }
    }
}
