﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Error = Assembler.ErrorException;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    class Module
    {
        private Assembler.Errors errPrinter = Assembler.Errors.GetInstance();
   
        public string ModuleName
        { get; private set; }

        Header headerRecord;

        public Header HeaderRecord
        { get { return headerRecord; } }

        Dictionary<int, Text> textRecords = new Dictionary<int,Text>();

        public int TotalTextRecords
        { get { return textRecords.Count; } }

        public Dictionary<int, Text> TextRecords
        { get { return textRecords; } }

        Dictionary<int, Linking> linkingRecords = new Dictionary<int,Linking>();

        public int TotalLinkingRecords
        { get { return linkingRecords.Count; } }

        public Dictionary<int, Linking> LinkingRecords
        { get { return linkingRecords; } }

        Dictionary<int, Modify> modifyRecords = new Dictionary<int,Modify>();

        public int TotalModifyRecords
        { get { return modifyRecords.Count; } }

        public Dictionary<int, Modify> ModifyRecords
        { get { return modifyRecords; } }

        int relocationValue;

        public int RelocateValue
        { get { return relocationValue; } }

        public Module() { }
        public Module(Header header)
        {
            headerRecord = header;
            ModuleName = header.ProgramName;
            relocationValue = header.LinkerLoadAddress - header.AssemblerLoadAddress;
        }

        public void AddRecord(Text rec)
        {
            // relocate the address as we add it
            int address = rec.Location;
            address += RelocateValue;

            if (!(0 <= address && address <= 1023))
            {
                // error, address will be relocated out of the range of memory
                throw new Error(ErrCat.Serious, 40);
            }

            if (address < HeaderRecord.LinkerLoadAddress || 
                address > HeaderRecord.LinkerLoadAddress + HeaderRecord.ModuleLength)
            {
                // error, address will be relocated out of the range of the module
                throw new Error(ErrCat.Serious, 41);
            }

            rec.Location = address;

            // add the record to the module
            if (textRecords.ContainsKey(rec.Location))
            {
                // error, multiple text records with same location counter value
                throw new Error(ErrCat.Serious, 42);
            }
            else
            {
                if (rec.Flag == 'R')
                {
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
                        errPrinter.PrintError(ErrCat.Serious, 43);
                        rec.Word = 32768;
                    }
                }
                textRecords.Add(rec.Location, rec);
            }
        }

        public void AddRecord(Linking rec)
        {
            // relocate the address as we add it
            int address = rec.Location;
            address += RelocateValue;

            if (!(0 <= address && address <= 1023))
            {
                // error, address will be relocated out of the range of memory
                throw new Error(ErrCat.Serious, 44);
            }

            if (address < HeaderRecord.LinkerLoadAddress ||
                address > HeaderRecord.LinkerLoadAddress + HeaderRecord.ModuleLength)
            {
                // error, address will be relocated out of the range of the module
                throw new Error(ErrCat.Serious, 45);
            }

            rec.Location = address;
            

            // add the record to the module
            if (linkingRecords.ContainsKey(rec.Location))
            {
                // error, multiple linking records with same location counter value
                throw new Error(ErrCat.Serious, 46);
            }
            else
            {
                linkingRecords.Add(rec.Location, rec);
            }
        }

        public void AddRecord(Modify rec)
        {
            // relocate the address as we add it
            int address = rec.Location;
            address += RelocateValue;

            if (!(0 <= address && address <= 1023))
            {
                // error, address will be relocated out of the range of memory
                throw new Error(ErrCat.Serious, 47);
            }

            if (address < HeaderRecord.LinkerLoadAddress ||
                address > HeaderRecord.LinkerLoadAddress + HeaderRecord.ModuleLength)
            {
                // error, address will be relocated out of the range of the module
                throw new Error(ErrCat.Serious, 48);
            }

            rec.Location = address;

            // add the record to the module
            if (modifyRecords.ContainsKey(rec.Location))
            {
                // error, multiple modify records with same location counter value
                throw new Error(ErrCat.Serious, 49);
            }
            else
            {
                modifyRecords.Add(rec.Location, rec);
            }
        }

        public Text GetTextRecord(int LC)
        {
            if (textRecords.ContainsKey(LC))
            {
                return textRecords[LC];
            }
            return null;
        }
    }
}
