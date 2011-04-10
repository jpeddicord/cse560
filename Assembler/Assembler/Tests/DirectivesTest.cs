using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class DirectivesTest
    {
        /**
         * [D1] Ensures Contains() works for a known directive.
         */
        [Test]
        public void DirectiveExists()
        {
            var d = Directives.GetInstance();
            Assert.IsTrue(d.Contains("START"));
        }

        /**
         * [D2] Ensures Contains() returns the correct result for a directive that does not exist.
         */
        [Test]
        public void DirectiveDoesntExist()
        {
            var d = Directives.GetInstance();
            Assert.IsFalse(d.Contains("derp"));
        }

        /**
         * [D3] Tests that case does not make a difference.
         */
        [Test]
        public void DirectiveCaseChange()
        {
            var d = Directives.GetInstance();
            Assert.IsTrue(d.Contains("EnD"));
        }

        /**
         * [D4] Ensures that all directives are being read from the file and stored in Directives.  Also
         * checks that DirectiveCount returns the correct number of items.
         */
        [Test]
        public void DirectiveContainsAll()
        {
            var d = Directives.GetInstance();
            Assert.AreEqual(11, d.DirectiveCount);
        }

        /**
         * [D5] Further testing that all directives are read in by checking for the first and last directive
         * in the file.
         */
        [Test]
        public void DirectiveFirstLast()
        {
            var d = Directives.GetInstance();
            Assert.IsTrue(d.Contains("start") && d.Contains("NOP"));
        }
    }
}
