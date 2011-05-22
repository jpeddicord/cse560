using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Linker
{
    class LoadFile
    {
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
                    }
/*
                    if (!(0 <= s && s <= 1023))
                    {
                        // Error: Address Field modification will be out of range of this program
                        // NOP substituted
                        word = 32768;
                    }
 **/
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

        public void Render(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);

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
            }

            loadHeader.ProgramName = modules[0].HeaderRecord.ProgramName;
            loadHeader.LinkerLoadAddress = modules[0].HeaderRecord.LinkerLoadAddress;
            loadHeader.ExecutionStartAddress = loadHeader.LinkerLoadAddress;
            loadHeader.ModuleLength = totalLength;
            loadHeader.TotalRecords = totalTextRecords + 2;
            loadHeader.TotalTextRecords = totalTextRecords;

            // write header record to file
            writer.WriteLine(loadHeader.ToString());

            // write the text records
            WriteTextRecords(writer);

            // write the end record
            writer.WriteLine(String.Format("E:{0}", modules[0].HeaderRecord.ProgramName));

            // close the file
            writer.Close();
        }
    }
}
