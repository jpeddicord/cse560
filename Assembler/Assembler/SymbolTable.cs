using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assembler
{
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

    public class SymbolTable
    {

        private Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            Trace.WriteLine("Creating symbol table", "SymbolTable");   
            this.symbols = new Dictionary<string, Symbol>();
        }

        public void AddSymbol(Symbol symbol)
        {
            Trace.WriteLine(String.Format("Adding {0} to symbol table.", symbol.rlabel), "SymbolTable");
            this.symbols[symbol.rlabel] = symbol;
        }

        public void AddSymbol(string rlabel, string lc, Usage usage, string val)
        {
            Symbol sym;
            sym.rlabel = rlabel;
            sym.lc = lc;
            sym.usage = usage;
            sym.val = val;
            this.AddSymbol(sym);
        }

        public Symbol GetSymbol(string rlabel)
        {
            return this.symbols[rlabel];
        }

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

