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

        public override string ToString()
        {
            string disp = "\n\n\t";
            disp += "----------- SYMBOL TABLE -----------\n";
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
