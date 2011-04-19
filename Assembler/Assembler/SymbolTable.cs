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
        LABEL,
        ENTRY,
        PRGMNAME,
        EXTERNAL,
        EQUATED
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
         * Create a new symbol table. It is initially empty.
         *
         * @refcode
         * @errtest
         *  See the test plan for SymbolTable for information.
         * @errmsg
         *  None known.
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
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
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public void AddSymbol(Symbol symbol)
        {
            Logger.Log(String.Format("Adding {0} to symbol table.", symbol.rlabel), "SymbolTable");
            if (this.symbols.ContainsKey(symbol.rlabel))
            {
                throw new SymbolException("Duplicate symbol in table.");
            }
            this.symbols[symbol.rlabel] = symbol;
        }

        /**
         * Add a symbol to the symbol table. Constructs a Symbol based on the
         * input parameters, and adds it to the table.
         *
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         * 
         * 
         * @param rlabel Label for the constructed symbol
         * @param lc Location counter for the symbol
         * @param usage Usage information for the symbol
         * @param val Optional value for the symbol
         */
        public void AddSymbol(string rlabel, string lc, Usage usage, string val = null)
        {
            Symbol sym;
            sym.rlabel = rlabel;
            sym.lc = lc;
            sym.usage = usage;
            sym.val = val;
            this.AddSymbol(sym);
        }

        /**
         * TODO: DOCUMENT MEEEE
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
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
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
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 17, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public bool ContainsSymbol(string rlabel)
        {
            return this.symbols.ContainsKey(rlabel);
        }

        /**
         * Get a list of symbols in the table, sorted by label.
         *
         * @return sorted list of symbol labels
         * @refcode
         * @errtest
         *  Tested sorting of symbols
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
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
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         *  - April 18, 2011 - Jacob - Fix display of missing LC
         *  - April 19, 2011 - Jacob - Display value if available
         * @codestandard Mark
         * @teststandard Andrew
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

