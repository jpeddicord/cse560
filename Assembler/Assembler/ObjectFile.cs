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

        public ObjectFile(IntermediateFile input, SymbolTable symb)
        {
            this.input = input;
            this.symb = symb;

            // extract the program name

        }

        /**
         * Add a linking record to this object file.
         */
        public void AddLinkingRecord(LinkingRecord record)
        {
            this.linkingRecords.Add(record);
        }

        /**
         * Add a text record to this object file.
         */
        public void AddLinkingRecord(TextRecord record)
        {
            this.textRecords.Add(record);
        }

        /**
         * Add a modification record to this object file.
         */
        public void AddLinkingRecord(ModificationRecord record)
        {
            this.modificationRecords.Add(record);
        }

        /**
         * Render the object file into the file given. If no filename, will
         * print to stdout.
         */
        public void Render(string filename)
        {
            var file = new StreamWriter(filename);

            var header = new HeaderRecord("FIXME");
            // TODO: fill out header
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

            var end = new EndRecord("FIXME");
            file.WriteLine(end);

        }
    }
}

