using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Usage = Assembler.Usage;

namespace Linker
{
    class Symbol
    {
        public string Label
        { get; private set; }

        public Usage SymbolUsage
        { get; private set; }

        public int LinkerValue
        { get; private set; }

        public Symbol(string label, Usage use, int linkerValue)
        {
            Label = label;
            SymbolUsage = use;
            LinkerValue = linkerValue;
        }
    }

    class SymbolTable
    {
        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

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

        public void AddSymbol(string label, Usage use, int linkervalue)
        {
            Symbol sym = new Symbol(label, use, linkervalue);
            AddSymbol(sym);
        }

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

        public bool ContainsSymbol(string label)
        {
            if (label != null)
                return this.symbols.ContainsKey(label);
            else
                return false;
        }
    }
}
