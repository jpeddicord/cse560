using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    /**
     * Represents the Load file output of the Linker.
     */
    class LoadFile
    {
        /**
         * Allows the LoadFile to report errors.
         */
        Assembler.Errors errPrinter = Assembler.Errors.GetInstance();

        /**
         * The modules that are included in this LoadFile.
         */
        private List<Module> modules;

        /**
         * The SymbolTable containing Symbols that are needed for this LoadFile.
         */
        private SymbolTable symb;

        /**
         * The header of this LoadFile.
         */
        private Header loadHeader = new Header();

        /**
         * Creates a LoadFile containing Modules and a SymbolTable.
         * 
         * @param modules the modules included in this LoadFile
         * @param symb the SymbolTable holding the Symbols needed for this LoadFile
         * 
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public LoadFile(List<Module> modules, SymbolTable symb)
        {
            this.modules = modules;
            this.symb = symb;
        }

        /**
         * Looks at the modify records and does the adjustments in the records.
         *
         * @param mod the Module whose modify records should be calculated
         * @param symb the SymbolTable that contains the symbols in the specified Module
         * 
         * @refcode
         *  OB4
         * @errtest
         * @errmsg
         *  ES.50, ES.51, ES.56
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

                try
                {
                    mod.GetTextRecord(mRec[rec.Key].Location).Word = word;
                }
                catch (NullReferenceException)
                {
                    errPrinter.PrintError(ErrCat.Serious, 56);
                }
            }
        }

        /**
         * Writes text records to a file using the parameter StreamWriter.
         * 
         * @param writer the stream to write to
         * 
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Prints text records to the screen.
         * 
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 23, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Creates and writes the LoadFile to a file.
         * 
         * @param filename the name the LoadFile will be written to disk as.
         * 
         * @refcode
         *  LM
         * @errtest
         * @errmsg
         *  ES.52, ES.53, ES.54
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

            if (totalLength < totalTextRecords)
            {
                // error, should have the same number of text records as
                // the length of the program
                errPrinter.PrintError(ErrCat.Serious, 53);
            }

            if (totalTextRecords > 1024)
            {
                // error, program exceeds memory
                errPrinter.PrintError(ErrCat.Serious, 58);
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
