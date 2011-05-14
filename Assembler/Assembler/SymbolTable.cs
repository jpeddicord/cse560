using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assembler
{
    /**
     * The type of symbol.
     */
    public enum Usage
    {
        /**
         * A label on a line.
         */
        LABEL,

        /**
         * An exported entry symbol.
         */
        ENTRY,

        /**
         * The program name.
         */
        PRGMNAME,

        /**
         * An externally-located symbol.
         */
        EXTERNAL,

        /**
         * An equated symbol.
         */
        EQUATED,
    }

    /**
     * Exception thrown for invalid symbol operations.
     */
    public class SymbolException : ApplicationException
    {
        public SymbolException() : base() {}
        public SymbolException(string s) : base(s) {}
        public SymbolException(string s, Exception ex) : base(s, ex) {}
    }

    /**
     * A structure that represents a symbol in the symbol table.
     */
    public struct Symbol
    {
        /**
         * The associated label with the symbol.
         */
        public string rlabel;

        /**
         * The location counter, if applicable.
         */
        public string lc;

        /**
         * The symbol's usage. See the Usage enumeration for options.
         */
        public Usage usage;

        /**
         * The optional stored value of the symbol. Most useful for equated
         * symbols.
         */
        public string val;

        /**
         * For equated symbols, the number of relocations that are required
         * if this symbol is used.
         */
        public int relocations;
    }

    /**
     * A symbol table. Capable of storing symbols by their rlabel, and keeping
     * associated information (LC, usage, value) intact. Can output the table
     * in a sorted fashion.
     */
    public class SymbolTable
    {

        /**
         * Representation; holds the label to symbol mapping.
         */
        private Dictionary<string, Symbol> symbols;

        /**
         * The extracted program name. Useful, since we only have one.
         */
        public string ProgramName { get; private set; }

        /**
         * Create a new symbol table. It is initially empty.
         *
         * @refcode S2
         * @errtest
         *  See the test plan for SymbolTable for information.
         * @errmsg
         *  None known.
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public SymbolTable()
        {
            Logger.Log("Creating symbol table", "SymbolTable");   
            this.symbols = new Dictionary<string, Symbol>();
        }

        /**
         * Add a symbol to the symbol table. Takes a single Symbol object.
         *
         * @param symbol Symbol object to add to the table
         *
         * @refcode S2
         * @errtest
         *  Adding of various symbol types
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         *  - May 7, 2011 - Jacob - Store the program name in a more accessible location.
         *  - May 13, 2011 - Jacob - Coerce val & lc into uppercase
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void AddSymbol(Symbol symbol)
        {
            Logger.Log(String.Format("Adding {0} to symbol table.", symbol.rlabel), "SymbolTable");

            // check for existence
            if (this.symbols.ContainsKey(symbol.rlabel))
            {
                throw new SymbolException("Duplicate symbol in table.");
            }

            // coerce into uppercase
            if (symbol.lc != null)
            {
                symbol.lc = symbol.lc.ToUpper();
            }
            if (symbol.val != null)
            {
                symbol.val = symbol.val.ToUpper();
            }

            //store it
            this.symbols[symbol.rlabel] = symbol;

            // store the program name if given
            if (symbol.usage == Usage.PRGMNAME)
            {
                this.ProgramName = symbol.rlabel;
            }
        }

        /**
         * Add a symbol to the symbol table. Constructs a Symbol based on the
         * input parameters, and adds it to the table.
         *
         * @param rlabel Label for the constructed symbol
         * @param lc Location counter for the symbol
         * @param usage Usage information for the symbol
         * @param val Optional value for the symbol
         * @param relocations Number of relocations needed if this equated symbol is used
         *
         * @refcode S2
         * @errtest
         *  Adding of various symbols
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         *  - May 13, 2011 - Jacob - Added relocations
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void AddSymbol(string rlabel, string lc, Usage usage, string val = null, int relocations = 0)
        {
            Symbol sym;
            sym.rlabel = rlabel;
            sym.lc = lc;
            sym.usage = usage;
            sym.val = val;
            sym.relocations = relocations;
            this.AddSymbol(sym);
        }

        /**
         * Remove a symbol from the table, returning it.
         *
         * @param rLabel symbol to remove by label
         * @return the removed symbol
         *
         * @refcode S2
         * @errtest
         *  Removing of a symbol
         * @errmsg
         *  None
         * @author Mark Mathis
         * @creation April 19, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Symbol RemoveSymbol(string rLabel)
        {
            Symbol value;
            if (this.symbols.ContainsKey(rLabel))
            {
                value = symbols[rLabel];
                symbols.Remove(rLabel);
            }
            else
            {
                throw new SymbolException("Symbol doesn't exist.");
            }

            return value;
        }

        /**
         * Get a single symbol by label.
         *
         * @param rlabel symbol to look up
         * @return the associated symbol
         *
         * @refcode S2
         * @errtest
         *  Checking the values of added symbols
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Symbol GetSymbol(string rlabel)
        {
            if (this.symbols.ContainsKey(rlabel))
            {
                return this.symbols[rlabel];
            }
            else
            {
                throw new SymbolException("Symbol doesn't exist.");
            }
        }

        /**
         * Return whether the symbol table contains the given symbol.
         *
         * @param rlabel symbol to look up
         * @return true if it exists, false if not
         *
         * @refcode S2
         * @errtest
         *  Checking if symbols exist
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 17, 2011
         * @modlog
         *  - May 13, 2011 - Mark - Now returns false if rlabel is null.
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public bool ContainsSymbol(string rlabel)
        {
            if (rlabel != null)
                return this.symbols.ContainsKey(rlabel);
            else
                return false;
        }

        /**
         * Get a list of symbols in the table, sorted by label.
         *
         * @return sorted list of symbol labels
         *
         * @refcode S2
         * @errtest
         *  Tested sorting of symbols
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         * 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public List<string> SortedSymbols()
        {
            var keys = new List<string>();
            foreach (var key in this.symbols.Keys)
            {
                keys.Add(key);
            }
            keys.Sort();
            return keys;
        }

        /**
         * Get the symbol table for pretty-printing. Displays, for each symbol:
         *  - rlabel
         *  - location counter
         *  - usage
         *  - value
         *
         * @return pretty-printed string of the table
         *
         * @refcode S2
         * @errtest
         *  Display of output in sample test scripts
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         *  - April 18, 2011 - Jacob - Fix display of missing LC
         *  - April 19, 2011 - Jacob - Display value if available
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public override string ToString()
        {
            string disp = "---- SYMBOL TABLE ----\n";
            foreach (string s in this.SortedSymbols())
            {
                Symbol sym = this.symbols[s];
                disp += String.Format("{0,32}: {1,-8} {2,-8} {3}\n",
                                      sym.rlabel,
                                      sym.lc != null ? String.Format("0x{0}", sym.lc) : "  ",
                                      sym.usage,
                                      sym.val != null ? sym.val : "");
            }
            return disp;
        }

    }
}

