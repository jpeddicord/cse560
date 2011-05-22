using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Module
    {
        public string ModuleName
        { get; private set; }

        Header headerRecord;

        Dictionary<int, Text> textRecords = new Dictionary<int,Text>();

        Dictionary<int, Linking> linkingRecords = new Dictionary<int,Linking>();

        Dictionary<int, Modify> modifyRecords = new Dictionary<int,Modify>();

        int relocationValue;

        public Module() { }
        public Module(Header header)
        {
            headerRecord = header;
            ModuleName = header.ProgramName;
            relocationValue = header.LinkerLoadAddress - header.AssemblerLoadAddress;
        }

        public void AddRecord(Text rec)
        {
            if (textRecords.ContainsKey(rec.Location))
            {
                // error, multiple text records with same location counter value
            }
            else
            {
                if (rec.Flag == 'R')
                {
                    //TODO is this right?
                    //rec.Word += this.relocationValue;
                    int s = Convert.ToInt32(Convert.ToString(rec.Word, 2).Substring(6), 2);
                    s += this.relocationValue;
                    if (0 <= s && s <= 1023)
                    {
                        rec.Word += this.relocationValue;
                    }
                    else
                    {
                        // Error: Address Field relocation will be out of range of this program
                        // NOP substituted
                        rec.Word = 32768;
                    }
                }
                textRecords.Add(rec.Location, rec);
            }
        }

        public void AddRecord(Linking rec)
        {
            if (linkingRecords.ContainsKey(rec.Location))
            {
                // error, multiple linking records with same location counter value
            }
            else
            {
                linkingRecords.Add(rec.Location, rec);
            }
        }

        public void AddRecord(Modify rec)
        {
            if (modifyRecords.ContainsKey(rec.Location))
            {
                // error, multiple modify records with same location counter value
            }
            else
            {
                modifyRecords.Add(rec.Location, rec);
            }
        }
    }
}
