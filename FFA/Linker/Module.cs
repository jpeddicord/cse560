using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Error = Assembler.ErrorException;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    /**
     * Represents a part of a program in the FFA language. The parsed information
     * from a single file of Assembler output is saved in a Module.
     */
    class Module
    {
        /**
         * Allows the use of the Assembler.Errors class to report errors.
         */
        private Assembler.Errors errPrinter = Assembler.Errors.GetInstance();
   
        /**
         * The name of the Module as specified in the header record.
         */
        public string ModuleName
        { get; private set; }

        /**
         * The header record that corresponds to this Module.
         */
        Header headerRecord;

        /**
         * Public accessor to this Module's header record.
         */
        public Header HeaderRecord
        { get { return headerRecord; } }

        /**
         * All text records contained in this Module.
         */
        Dictionary<int, Text> textRecords = new Dictionary<int,Text>();

        /**
         * The total number of text records contained in this Module.
         */
        public int TotalTextRecords
        { get { return textRecords.Count; } }

        /**
         * Public accessor to this Module's text records.
         */
        public Dictionary<int, Text> TextRecords
        { get { return textRecords; } }

        /**
         * The linking records that are contained in this Module.
         */
        Dictionary<int, Linking> linkingRecords = new Dictionary<int,Linking>();

        /**
         * The total number of linking records contained in this Module.
         */
        public int TotalLinkingRecords
        { get { return linkingRecords.Count; } }

        /**
         * Public accessor to this Module's linking records.
         */
        public Dictionary<int, Linking> LinkingRecords
        { get { return linkingRecords; } }

        /**
         * The modify records that are contained in this Module.
         */
        Dictionary<int, Modify> modifyRecords = new Dictionary<int,Modify>();

        /**
         * The total number of modify records contained in this Module.
         */
        public int TotalModifyRecords
        { get { return modifyRecords.Count; } }

        /**
         * Public accessor to this Module's modify records.
         */
        public Dictionary<int, Modify> ModifyRecords
        { get { return modifyRecords; } }

        /**
         * The value that this Module is being relocated by the Linker.
         */
        int relocationValue;

        /**
         * Public accessor to this Module's relocation value.
         */
        public int RelocateValue
        { get { return relocationValue; } }

        /**
         * Empty constructor to be used for instantiating placeholder Modules.
         * 
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public Module() { }
        
        /**
         * Constructs Module object to hold the program corresponding with the
         * indicated header record
         * 
         * @param header the Header record for this module
         * 
         * @refcode
         *  OB, LM
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public Module(Header header)
        {
            headerRecord = header;
            ModuleName = header.ProgramName;
            relocationValue = header.LinkerLoadAddress - header.AssemblerLoadAddress;
        }

        /**
         * Adds the indicated text record to the Module.
         * 
         * @param rec the text record to be added
         * 
         * @refcode
         * OB, LM
         * @errtest
         * @errmsg
         *  ES.40, ES.41, ES.42
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Adds the indicated linking record to the Module.
         * 
         * @param rec the linking record to be added
         * 
         * @refcode
         * OB, LM
         * @errtest
         * @errmsg
         *  ES.44, ES.45, ES.46
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Adds the indicated modify record to the Module.
         * 
         * @param rec the modify record to be added
         * 
         * @refcode
         * OB, LM
         * @errtest
         * @errmsg
         *  ES.47, ES.48, ES.49
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Returns the indicated text record.
         * 
         * @param LC the location counter value of the text record to return
         * @return the text record at the indicated location
         * 
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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
