using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax;
using System.IO;
using System.Text;

namespace Mnemo.Tests.Core.Syntax
{
    [TestClass]
    public class TestMnemoPreTokenizer
    {


        [TestMethod]
        public void MnemoPreTokenizer_ShouldSeparatePreTokensCorrectly()
        {
            string testCase = "const getPlayerById(id: int32_t) => Read<int32_t>(PLAYER_ADDRESS, 0x20 + id * 8);";
            string[] expected = { "const", "getPlayerById", "(", "id", ":", "int32_t", ")", "=>", "Read", "<", "int32_t", ">", "(", "PLAYER_ADDRESS", ",", "0x20", "+", "id", "*", "8", ")", ";" };

            using MemoryStream stream = new(Encoding.UTF8.GetBytes(testCase));
            using StreamReader reader = new(stream);
            MnemoPreTokenizer preTokenizer = new(reader);

            string[] preTokens = preTokenizer.Process();

            CollectionAssert.AreEqual(expected, preTokens);
        }
    }
}
