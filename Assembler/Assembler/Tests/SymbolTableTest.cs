using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class SymbolTableTest
    {
        [Test]
        public void AddSymbolStruct()
        {
            var t = new SymbolTable();
            Symbol sym;
            sym.rlabel = "test";
            sym.lc = "5";
            sym.usage = Usage.EQUATED;
            sym.val = "derp";
            t.AddSymbol(sym);
            Assert.AreEqual(sym, t.GetSymbol("test"));
        }

        [Test]
        public void AddSymbolParams()
        {
            var t = new SymbolTable();
            Symbol sym;
            sym.rlabel = "test";
            sym.lc = "5";
            sym.usage = Usage.EQUATED;
            sym.val = "derp";
            t.AddSymbol("test", "5", Usage.EQUATED, "derp");
            Assert.AreEqual(sym, t.GetSymbol("test"));
        }

        [Test]
        public void Sorting()
        {
            var t = new SymbolTable();
            t.AddSymbol("apple", "5", Usage.EQUATED, "derp");
            t.AddSymbol("pear", "20", Usage.ENTRY, "");
            t.AddSymbol("orange", "42", Usage.ENTRY, "");
            var l = t.SortedSymbols();
            Assert.AreEqual(l[0], "apple");
            Assert.AreEqual(l[1], "orange");
            Assert.AreEqual(l[2], "pear");
        }

    }
}

