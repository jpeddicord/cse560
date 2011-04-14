using System;
using NUnit.Framework;

namespace Assembler
{
    [TestFixture]
    public class BinaryHelperTest
    {
        /**
         * [B1] Testing conversion of a negative number to its two's complement representation with 10 digits.
         */
        [Test]
        public void BHToTwosComp10Digits()
        {
            Assert.AreEqual(960, BinaryHelper.ConvertNumber(-64, 10));
        }

        /**
         * [B2] Testing conversion of a negative number to its two's complement representation with 16 digits.
         */
        [Test]
        public void BHToTwosComp16Digits()
        {
            Assert.AreEqual(65472, BinaryHelper.ConvertNumber(-64, 16));
        }

        /**
         * [B3] Testing conversion of a two's complement number to its negative value with 10 digits.
         */
        [Test]
        public void BHFromTwosComp10Digits()
        {
            Assert.AreEqual(-64, BinaryHelper.ConvertNumber(960, 10));
        }

        /**
         * [B4] Testing conversion of a two's complement number to its negative value with 8 digits.
         */
        [Test]
        public void BHFromTwosComp8Digits()
        {
            Assert.AreEqual(-64, BinaryHelper.ConvertNumber(192, 8));
        }

        /**
         * [B5] Testing conversion of the smallest number to its negative two's complement representation with max digits (16).
         */
        [Test]
        public void BHToTwosComp16DigitsMin()
        {
            Assert.AreEqual(32768, BinaryHelper.ConvertNumber(-32768, 16));
        }

        /**
         * [B6] Testing conversion of a smallest two's complement to its negative corresponding value.
         */
        [Test]
        public void BHFromTwosComp16DigitsMin()
        {
            Assert.AreEqual(-32768, BinaryHelper.ConvertNumber(32768, 16));
        }

        /**
         * [B7] Testing conversion of the largest number to its negative two's complement representation with max digits (16).
         */
        [Test]
        public void BHToTwosComp16DigitsMax()
        {
            Assert.AreEqual(65535, BinaryHelper.ConvertNumber(-1, 16));
        }

        /**
         * [B8] Testing conversion of the largest two's complement to its negative corresponding value.
         */
        [Test]
        public void BHFromTwosComp16DigitsMax()
        {
            Assert.AreEqual(-1, BinaryHelper.ConvertNumber(65535, 16));
        }
    }
}
