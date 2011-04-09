using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class InstructionsTest
    {
        [Test]
        public void InstructionExists()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsInstruction("MOPER", "ADD"));
        }

        [Test]
        public void InstructionDoesntExists()
        {
            var i = Instructions.GetInstance();
            Assert.IsFalse(i.IsInstruction("moper", "derp"));
        }

        [Test]
        public void InstructionCaseChange()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsInstruction("Moper", "AdD"));
        }

    }
}

