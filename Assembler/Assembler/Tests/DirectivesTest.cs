using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class DirectivesTest
    {
        [Test]
        public void DirectiveExists()
        {
            var d = Directives.GetInstance();
            Assert.IsTrue(d.Contains("START"));
        }

        [Test]
        public void DirectiveDoesntExist()
        {
            var d = Directives.GetInstance();
            Assert.IsFalse(d.Contains("derp"));
        }

        [Test]
        public void DirectiveCaseChange()
        {
            var d = Directives.GetInstance();
            Assert.IsTrue(d.Contains("EnD"));
        }
    }
}
