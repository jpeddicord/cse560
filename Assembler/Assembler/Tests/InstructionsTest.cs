using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class InstructionsTest
    {
        /**
         * [I1] Tests that a known instruction is shown to exist.
         */
        [Test]
        public void InstructionExists()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsInstruction("MOPER", "ADD"));
        }

        /**
         * [I2] Tests that calling IsInstruction with an existing group but nonexisting instruction returns false.
         */
        [Test]
        public void InstructionDoesntExist()
        {
            var i = Instructions.GetInstance();
            Assert.IsFalse(i.IsInstruction("moper", "derp"));
        }

        /**
         * [I3] Tests that IsInstruction() is case insensitive.
         */
        [Test]
        public void InstructionCaseChange()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsInstruction("Moper", "AdD"));
        }

        /**
         * [I4] Tests that calling IsInstruction() with a nonexistant group but existing instruction returns false.
         */
        [Test]
        public void InstructionGroupDoesntExist()
        {
            var i = Instructions.GetInstance();
            Assert.IsFalse(i.IsInstruction("WhatEvenIs", "ADD"));
        }

        /**
         * [I5] Tests that calling IsInstruction() with a nonexistent group and nonexistent instruction returns false.
         */
        [Test]
        public void InstructionNeitherExist()
        {
            var i = Instructions.GetInstance();
            Assert.IsFalse(i.IsInstruction("WhatEvenIs", "IdontEven"));
        }

        /**
         * [I6] Tests that IsGroup() will return true for a known group
         */
        [Test]
        public void InstructionGroupExists()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsGroup("MOPER"));
        }

        /**
         * [I7] Tests that IsGroup() will return false for a group that does not exist.
         */
        [Test]
        public void InstructionNonexistentGroup()
        {
            var i = Instructions.GetInstance();
            Assert.IsFalse(i.IsGroup("MEM"));
        }

        /**
         * [I8] Tests that IsGroup() is case insensitive.
         */
        [Test]
        public void InstructionGroupCaseChange()
        {
            var i = Instructions.GetInstance();
            Assert.IsTrue(i.IsGroup("SoPEr"));
        }

        /**
         * [I9] Tests getBytecodeString() returns a valid conversion.
         */
        [Test]
        public void InstructionByteCode()
        {
            var i = Instructions.GetInstance();
            Assert.AreEqual("10101", i.GetBytecodeString("SOPER", "AND"));
        }

        /**
         * [I10] Tests that getBytecodeString() throws an exception when an invalid group is given.
         */
        [Test]
        public void InstructionByteCodeNoGroup()
        {
            var i = Instructions.GetInstance();

            try
            {
                i.GetBytecodeString("MEM", "ADD");
            }
            catch (InstructionException)
            {
                return;
            }

            Assert.Fail("Exception not thrown while accessing with non-existent group.");
        }

        /**
         * [I11] Tests that getBytecodeString() throws an exception when an invalid instruction is given.
         */
        [Test]
        public void InstructionByteCodeNoInstruction()
        {
            var i = Instructions.GetInstance();

            try
            {
                i.GetBytecodeString("SOPER", "CLEAR");
            }
            catch (InstructionException)
            {
                return;
            }

            Assert.Fail("Exception not thrown while accessing with non-existent instruction.");
        }

    }
}

