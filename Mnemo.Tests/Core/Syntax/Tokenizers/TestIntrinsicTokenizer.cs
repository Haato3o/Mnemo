using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax.Tokenizers;

namespace Mnemo.Tests.Core.Syntax.Tokenizers
{
    [TestClass]
    public class TestIntrinsicTokenizer
    {
        private IntrinsicsTokenizer tokenizer = new();

        [TestMethod]
        public void IntrinsicTokenizer_ShouldBeAbleToConvert()
        {
            string[] intrinsics = { "Read", "read" };
            bool[] expecteds = { true, false };

            for (int i = 0; i < intrinsics.Length; i++)
            {
                string intrinsic = intrinsics[i];
                bool expected = expecteds[i];
                var actual = tokenizer.CanTokenize(intrinsic);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
