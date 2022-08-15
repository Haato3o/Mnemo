using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Tokenizers;

namespace Mnemo.Tests.Core.Syntax.Tokenizers
{
    [TestClass]
    public class TestInt32Tokenizer
    {
        private readonly Int32Tokenizer tokenizer = new();

        [TestMethod]
        public void Int32Tokenizer_ShouldReturnRightCanTokenize()
        {
            string[] ints = { "10", "0b001010", "0x123FGH", "0x16A" };
            bool[] expecteds = { true, true, false, true };

            for (int i = 0; i < ints.Length; i++)
            {
                string preToken = ints[i];
                bool expected = expecteds[i];

                var actual = tokenizer.CanTokenize(preToken);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Int32Tokenizer_ShouldConvertCorrectly()
        {
            string[] ints = { "10", "0b001010", "0x16A" };
            int[] expecteds = { 10, 0b001010, 0x16A };

            for (int i = 0; i < ints.Length; i++)
            {
                string preToken = ints[i];
                int expected = expecteds[i];

                var actual = tokenizer.ConvertValue(preToken);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Int32Tokenizer_ShouldNotConvertNonValidValues()
        {
            string[] notInts = { "A", "Hello", "World", "0b02" };

            foreach (string notInt in notInts)
                Assert.IsFalse(tokenizer.CanTokenize(notInt), $"'{notInt}' returned true");
        }

        [TestMethod]
        public void Int32Tokenizer_ShouldBeValue()
        {
            Assert.AreEqual(Token.Value, tokenizer.Tokenize("10"));
        }
    }
}
