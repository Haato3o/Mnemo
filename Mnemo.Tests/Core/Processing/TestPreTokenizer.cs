using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Processing;
using System.IO;
using System.Text;

namespace Mnemo.Tests.Core.Processing
{
    [TestClass]
    public class TestPreTokenizer
    {

        [TestMethod]
        public void PreTokenizer_Should_SeparateTokenizeCorrectly()
        {
            string script =
            @"
                Address PLAYER_ADDRESS = 0x21000000;
                Offsets DYNAMIC_OFFSETS = [
                    0x10,
                    0x20,
                    (rbx * 8) + 0x20,
                    0xA8
                ];
                let PlayerId => Read<Int32>(PLAYER_ADDRESS, PLAYER_OFFSETS);
            ";

            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(script));
            using var sr = new StreamReader(ms);
            MnemoPreTokenizer tokenizer = new(sr);

            string[] tokens = tokenizer.PreTokenize();
            string[] expected =
            {
                "Address", "PLAYER_ADDRESS", "=", "0x21000000", ";",
                "Offsets", "DYNAMIC_OFFSETS", "=", "[", 
                "0x10", ",", 
                "0x20", ",", 
                "(", "rbx", "*", "8", ")", "+", "0x20", ",",
                "0xA8",
                "]", ";",
                "let", "PlayerId", "=>", "Read<Int32>", "(", "PLAYER_ADDRESS", ",", "PLAYER_OFFSETS", ")", ";"
            };

            CollectionAssert.AreEqual(tokens, expected);
        }
    }
}
