using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Tests.Utils;
using Mnemo.Core.Syntax.Stream;

namespace Mnemo.Tests.Core.Syntax.Stream
{
    [TestClass]
    public class TestTokenStream
    {
        [TestMethod]
        public void TokenStream_ShouldReadTokensCorrectly()
        {
            MnemoToken[] expected = RandomProvider.Random<MnemoToken>(16);

            TokenStream stream = new(expected);

            foreach (MnemoToken token in expected)
                Assert.AreEqual(stream.Read(), token);
        }
    }
}
