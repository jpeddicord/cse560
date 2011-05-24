using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usage = Assembler.Usage;

namespace Linker
{
    /**
     * Represents a symbol in the symbol table.
     */
    class Symbol
    {
        /**
         * The label used to refer to the symbol. Can be any valid label.
         */
        public string Label
        { get; private set; }

        /**
         * How the symbol is used. Can be any value of Assembler.Usage but
         * should only be PRGMNAME or ENTRY.
         */
        public Usage SymbolUsage
        { get; private set; }

        /**
         * The value the linker will need to relocate locations when
         * this Symbol is involved.
         */
        public int LinkerValue
        { get; private set; }

        /**
         * Constructs a Symbol object.
         * 
         * @param label the label this Symbol should be referred to
         * @param use the usage of this Symbol
         * @param linkerValue the value the Linker should use when this Symbol is needed
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
        public Symbol(string label, Usage use, int linkerValue)
        {
            Label = label;
            SymbolUsage = use;
            LinkerValue = linkerValue;
        }
    }

    /**
     * Holds all Symbols produced and needed by the Linker.
     */
    class SymbolTable
    {
        /**
         * Dictionary that holds the Symbols. Symbols can be retrieved by their label.
         */
        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        /**
         * Add a Symbol to the SymbolTable.
         * 
         * @param sym the Symbol to add
         * 
         * @refcode
         * @errtest
         * @errmsg
         *  SymbolException - "Duplicate symbol in table."
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void AddSymbol(Symbol sym)
        {
            if (!symbols.ContainsKey(sym.Label))
            {
                symbols.Add(sym.Label, sym);
            }
            else
            {
                throw new Assembler.SymbolException("Duplicate symbol in table.");
            }
        }

        /**
         * Add a Symbol to the SymbolTable.
         * 
         * @param label the label the Symbol to be added will have
         * @param use the use the Symbol to be added will have
         * @param linkerValue the linkerValue the Symbol to be added will have
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
        public void AddSymbol(string label, Usage use, int linkerValue)
        {
            Symbol sym = new Symbol(label, use, linkerValue);
            AddSymbol(sym);
        }

        /**
         * Removes and returns the specified Symbol from the SymbolTable.
         * 
         * @param label the label of the Symbol to remove
         * @return the Symbol specified for removal
         * 
         * @refcode
         * @errtest
         * @errmsg
         *  SymbolException - "Symbol doesn't exist."
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public Symbol RemoveSymbol(string label)
        {
            if (this.symbols.ContainsKey(label))
            {
                Symbol sym = symbols[label];
                symbols.Remove(label);
                return sym;
            }
            else
            {
                throw new Assembler.SymbolException("Symbol doesn't exist.");
            }
        }

        /**
         * Returns the specified Symbol.
         * 
         * @param label the Symbol to be returned
         * @return the Symbol specified
         * 
         * @refcode
         * @errtest
         * @errmsg
         *  SymbolException - "Symbol doesn't exist"
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public Symbol GetSymbol(string label)
        {
            if (this.symbols.ContainsKey(label))
            {
                return symbols[label];
            }
            else
            {
                throw new Assembler.SymbolException("Symbol doesn't exist.");
            }
        }

        /**
         * Determines whether or not the specified Symbol is in the SymbolTable.
         * 
         * @param label the Symbol in question
         * @return true if the Symbol exists in the SymbolTable
         *         false if the Symbol does not exist in the SymbolTable
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
        public bool ContainsSymbol(string label)
        {
            if (label != null)
                return this.symbols.ContainsKey(label);
            else
                return false;
        }

        /**
         * Returns a list of the Symbols in sorted order.
         * 
         * @return a list of the Symbols in the SymbolTable sorted alphabetically
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
        public List<string> SortedSymbols()
        {
            List<string> keys = new List<string>();
            foreach (var key in this.symbols.Keys)
            {
                keys.Add(key);
            }
            keys.Sort();
            return keys;
        }

        /**
         * Returns the SymbolTable in string format for easy output.
         * 
         * @return the SymbolTable in formatted string output
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
        public override string ToString()
        {
            string disp = "\t----------- SYMBOL TABLE -----------\n";
            disp += string.Format("{0,32}: {1,-16} {2,-8} \n\n",
                                      "Label",
                                      "Relocation value",
                                      "Symbol usage");
            foreach (string s in this.SortedSymbols())
            {
                Symbol sym = this.symbols[s];

                int val = sym.LinkerValue;
                val = Assembler.BinaryHelper.ConvertNumber(val);

                disp += string.Format("{0,32}: {1,-16} {2,-8} \n",
                                      sym.Label,
                                      Convert.ToString(val,16).ToUpper().PadLeft(4,'0'),
                                      sym.SymbolUsage);
            }
            return disp;
        }
    }
}
