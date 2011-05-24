using System;
using NUnit.Framework;

namespace Simulator
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the MOPER class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class MOPERTest
    {
        /**
         * [M1] Tests that Add works with both positive numbers.
         */
        [Test]
        public void AddPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, 10);
            MOPER.Run("ADD", "1100000000000001");
            Assert.AreEqual(15, m.GetDataStack()[0]);
        }

        /**
         * [M2] Tests that Add works with both negative numbers.
         */
        [Test]
        public void AddNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111111111010");
            m.SetWordInt(1, -10);
            MOPER.Run("ADD", "1100000000000001");
            Assert.AreEqual(65520, m.GetDataStack()[0]);
        }

        /**
         * [M3] Tests that Add works with a positive and negative number.
         */
        [Test]
        public void AddMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, -10);
            MOPER.Run("ADD", "1100000000000001");
            Assert.AreEqual(65531, m.GetDataStack()[0]);
        }

        /**
         * [M4] Tests that Sub works with both positive numbers.
         */
        [Test]
        public void SubPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, 10);
            MOPER.Run("SUB", "1100100000000001");
            Assert.AreEqual(65531, m.GetDataStack()[0]);
        }

        /**
         * [M5] Tests that Sub works with both negative numbers.
         */
        [Test]
        public void SubNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111111111010");
            m.SetWordInt(1, -10);
            MOPER.Run("SUB", "1100100000000001");
            Assert.AreEqual(4, m.GetDataStack()[0]);
        }

        /**
         * [M6] Tests that Sub works with a positive and negative number.
         */
        [Test]
        public void SubMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, -10);
            MOPER.Run("SUB", "1100100000000001");
            Assert.AreEqual(15, m.GetDataStack()[0]);
        }

        /**
         * [M7] Tests that Mul works with both positive numbers.
         */
        [Test]
        public void MulPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, 10);
            MOPER.Run("MUL", "1101000000000001");
            Assert.AreEqual(50, m.GetDataStack()[0]);
        }

        /**
         * [M8] Tests that Mul works with both negative numbers.
         */
        [Test]
        public void MulNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111111111010");
            m.SetWordInt(1, -10);
            MOPER.Run("MUL", "1101000000000001");
            Assert.AreEqual(60, m.GetDataStack()[0]);
        }

        /**
         * [M9] Tests that Mul works with a positive and negative number.
         */
        [Test]
        public void MulMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, -10);
            MOPER.Run("MUL", "1101000000000001");
            Assert.AreEqual(65486, m.GetDataStack()[0]);
        }

        /**
         * [M10] Tests that Div works with both positive numbers.
         */
        [Test]
        public void DivPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000100101");
            m.SetWordInt(1, 10);
            MOPER.Run("DIV", "1101100000000001");
            Assert.AreEqual(3, m.GetDataStack()[0]);
        }

        /**
         * [M11] Tests that Div works with both negative numbers.
         */
        [Test]
        public void DivNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111110111010");
            m.SetWordInt(1, -10);
            MOPER.Run("DIV", "1101100000000001");
            Assert.AreEqual(7, m.GetDataStack()[0]);
        }

        /**
         * [M12] Tests that Div works with a positive and negative number.
         */
        [Test]
        public void DivMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110001000101");
            m.SetWordInt(1, -10);
            MOPER.Run("DIV", "1101100000000001");
            Assert.AreEqual(65530, m.GetDataStack()[0]);
        }

        /**
         * [M13] Tests that Or works with both positive numbers.
         */
        [Test]
        public void OrPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, 10);
            MOPER.Run("OR", "1110000000000001");
            Assert.AreEqual(15, m.GetDataStack()[0]);
        }

        /**
         * [M14] Tests that Or works with both negative numbers.
         */
        [Test]
        public void OrNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111111111010");
            m.SetWordInt(1, -10);
            MOPER.Run("OR", "1110000000000001");
            Assert.AreEqual(65534, m.GetDataStack()[0]);
        }

        /**
         * [M15] Tests that Or works with a positive and negative number.
         */
        [Test]
        public void OrMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, -10);
            MOPER.Run("OR", "1110000000000001");
            Assert.AreEqual(65527, m.GetDataStack()[0]);
        }

        /**
         * [M16] Tests that And works with both positive numbers.
         */
        [Test]
        public void AndPos()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, 10);
            MOPER.Run("AND", "1110100000000001");
            Assert.AreEqual(0, m.GetDataStack()[0]);
        }

        /**
         * [M17] Tests that And works with both negative numbers.
         */
        [Test]
        public void AndNeg()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010111111111010");
            m.SetWordInt(1, -10);
            MOPER.Run("AND", "1110100000000001");
            Assert.AreEqual(65522, m.GetDataStack()[0]);
        }

        /**
         * [M18] Tests that And works with a positive and negative number.
         */
        [Test]
        public void AndMix()
        {
            Memory m = Memory.GetInstance();
            STACK.Run("PUSH", "0010110000000101");
            m.SetWordInt(1, -10);
            MOPER.Run("AND", "1110100000000001");
            Assert.AreEqual(4, m.GetDataStack()[0]);
        }
    }
}
