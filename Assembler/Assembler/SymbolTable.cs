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

    public struct Symbol
    {
        public string rlabel;
        public string lc;
        public Usage usage;
        public string val;
    }

    public class SymbolTable
    {

        private Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            Trace.WriteLine("Creating symbol table", "SymbolTable");   
            symbols = new Dictionary<string, Symbol>();
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

