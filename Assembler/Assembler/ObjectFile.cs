using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler
{
    public class ObjectFile
    {

        private List<LinkingRecord> linkingRecords = new List<LinkingRecord>();

        private List<TextRecord> textRecords = new List<TextRecord>();

        private List<ModificationRecord> modificationRecords = new List<ModificationRecord>();

        private IntermediateFile input;

        private SymbolTable symb;

        public ObjectFile(ref IntermediateFile input, ref SymbolTable symb)
        {
            this.input = input;
            this.symb = symb;
        }

        /**
         * Add a linking record to this object file.
         */
        private void AddRecord(LinkingRecord record)
        {
            this.linkingRecords.Add(record);
        }

        /**
         * Add a text record to this object file.
         */
        private void AddRecord(TextRecord record)
        {
            this.textRecords.Add(record);
        }

        /**
         * Add a modification record to this object file.
         */
        private void AddRecord(ModificationRecord record)
        {
            this.modificationRecords.Add(record);
        }

        /**
         * Scan the symbol table for ENTRY symbols and create linking records
         * for them.
         */
        private void GenerateLinkingRecords()
        {
            // iterate the symbol table
            foreach (string symbName in this.symb.SortedSymbols())
            {
                // we only care about ENTRY symbols
                Symbol symb = this.symb.GetSymbol(symbName);
                if (symb.usage == Usage.ENTRY)
                {
                    // generate a linking record
                    var record = new LinkingRecord(this.symb.ProgramName);
                    record.EntryName = symb.rlabel;
                    record.ProgramLocation = symb.lc;
                    this.AddRecord(record);
                }
            }
        }

        /**
         * Scan and the source (pass 2) and generate text and modification
         * records as apporpriate.
         */
        private void GenerateSourceRecords()
        {
            foreach (IntermediateLine line in this.input)
            {
                // only scan lines that will actually be in the output
                if (line.ProgramCounter == null)
                {
                    continue;
                }
                // TODO: magic
            }
        }

        /**
         * Render the object file into the file given. If no filename, will
         * print to stdout.
         */
        public void Render(string filename)
        {
            // generate records
            this.GenerateLinkingRecords();
            this.GenerateSourceRecords();

            // write out the file
            var file = new StreamWriter(filename);

            // generate and write the header record
            var header = new HeaderRecord(this.symb.ProgramName);
            header.LoadAddress = "0"; // FIXME: this isn't right.
            header.ModuleLength = "0"; // FIXME
            header.ExecutionStart = this.symb.GetSymbol(this.symb.ProgramName).lc;
            header.TotalLinking = Convert.ToString(this.linkingRecords.Count, 16);
            header.TotalText = Convert.ToString(this.textRecords.Count, 16);
            header.TotalModification = Convert.ToString(this.modificationRecords.Count, 16);
            header.TotalRecords = Convert.ToString(this.linkingRecords.Count +
                                                   this.textRecords.Count +
                                                   this.modificationRecords.Count, 16);
            file.WriteLine(header);

            // write linking records
            foreach (var record in this.linkingRecords)
            {
                file.WriteLine(record);
            }

            // write text records
            foreach (var record in this.textRecords)
            {
                file.WriteLine(record);
            }

            // write modification records
            foreach (var record in this.modificationRecords)
            {
                file.WriteLine(record);
            }

            // write the end record
            var end = new EndRecord(this.symb.ProgramName);
            file.WriteLine(end);

            file.Close();
        }
    }
}

