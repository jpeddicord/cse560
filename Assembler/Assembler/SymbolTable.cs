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
        ENTRY,
        PRGMNAME,
        EXTERNAL,
        EQUATED
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
            Trace.WriteLine("Creating symbol table", "SymbolTable");   
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
            Trace.WriteLine(String.Format("Adding {0} to symbol table.", symbol.rlabel), "SymbolTable");
            this.symbols[symbol.rlabel] = symbol;
        }

        /**
         * Add a symbol to the symbol table. Constructs a Symbol based on the
         * input parameters, and adds it to the table.
         *
         * @param rlabel Label for the constructed symbol
         * @param lc Location counter for the symbol
         * @param usage Usage information for the symbol
         * @param val Optional value for the symbol
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public void AddSymbol(string rlabel, string lc, Usage usage, string val)
        {
            Symbol sym;
            sym.rlabel = rlabel;
            sym.lc = lc;
            sym.usage = usage;
            sym.val = val;
            this.AddSymbol(sym);
        }

        /**
         * Get a single symbol by label.
         *
         * @param rlabel Label to look up
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
            return this.symbols[rlabel];
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
         *
         * @return pretty-printed string of the table
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public override string ToString()
        {
            string disp = "---- SYMBOL TABLE ----\n";
            foreach (string s in this.SortedSymbols())
            {
                Symbol sym = this.symbols[s];
                disp += String.Format("{0,10}: 0x{1,-6} {2}\n", sym.rlabel, sym.lc, sym.usage);
            }
            return disp;
        }

    }
}

