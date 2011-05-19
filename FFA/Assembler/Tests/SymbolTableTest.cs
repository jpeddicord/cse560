using System;
using NUnit.Framework;

namespace Assembler
{

    /**
     * List of tests designed specifically to check correct functionality of procedures in the SymbolTable class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class SymbolTableTest
    {
        /**
         * [S1] Tests adding a symbol by structure works.
         */
        [Test]
        public void AddSymbolStruct()
        {
            var t = new SymbolTable();
            Symbol sym;
            sym.rlabel = "test";
            sym.lc = "5";
            sym.usage = Usage.EQUATED;
            sym.val = "DERP";
            sym.relocations = 0;
            t.AddSymbol(sym);
            Assert.AreEqual(sym, t.GetSymbol("test"));
        }

        /**
         * [S2] Test adding a symbol by parameters.
         */
        [Test]
        public void AddSymbolParams()
        {
            var t = new SymbolTable();
            Symbol sym;
            sym.rlabel = "test";
            sym.lc = "5";
            sym.usage = Usage.EQUATED;
            sym.val = "DERP";
            sym.relocations = 0;
            t.AddSymbol("test", "5", Usage.EQUATED, "derp");
            Assert.AreEqual(sym, t.GetSymbol("test"));
        }

        /**
         * [S3] Test the sorted output of symbols.
         */
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

        /**
         * [S4] Test that an empty table is indeed empty.
         */
        [Test]
        public void EmptyTable()
        {
            var t = new SymbolTable();
            Assert.AreEqual(0, t.SortedSymbols().Count);
        }

        /**
         * [S5] Test that adding a duplicate symbol fails.
         */
        [Test]
        public void DuplicateSymbol()
        {
            var t = new SymbolTable();
            try
            {
                t.AddSymbol("one", "0", Usage.ENTRY, "");
                t.AddSymbol("one", "0", Usage.ENTRY, "");
            }
            catch (SymbolException)
            {
                return;
            }
            Assert.Fail();
        }

        /**
         * [S6] Test that looking up a nonexisting symbol fails.
         */
        [Test]
        public void NonexistingSymbol()
        {
            var t = new SymbolTable();
            try
            {
                t.GetSymbol("herp");
            } catch (SymbolException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}

