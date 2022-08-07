using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using Mnemo.Core.Utils;
using System.Linq;

namespace Mnemo.Tests.Core.Tokenizer
{
    [TestClass]
    public class TestMnemoLexer
    {

        private static ITokenizer[] tokenizers = Dependencies.FindAllTokenizers();
        private MnemoLexer lexer = new(tokenizers);

        [TestMethod]
        public void MnemoLexer_ShouldTokenizeCorrectly()
        {
            // const getPlayerById(id: int32_t) => Read<int32_t>(PLAYER_ADDRESS, 0x20 + id * 8);
            string[] preTokens =
            {
                "const", "getPlayerById", "(", "id", ":", "int32_t", ")", "=>", "Read", "<", "int32_t", ">", "(", "PLAYER_ADDRESS", ",", "0x20", "+", "id", "*", "8", ")", ";"
            };
            Token[] tokens =
            {
                Token.DefineConst, Token.Literal, Token.ParenStart, Token.Literal, Token.Colon, Token.Type, Token.ParenEnd, Token.Func, Token.Intrinsic,
                Token.LT, Token.Type, Token.GT, Token.ParenStart, Token.Literal, Token.Separator, Token.Value, Token.Arithmetic, Token.Literal, Token.Arithmetic,
                Token.Value, Token.ParenEnd, Token.End
            };

            var mnemoTokens = lexer.Invoke(preTokens)
                                   .Select(token => token.Token)
                                   .ToArray();

            CollectionAssert.AreEqual(tokens, mnemoTokens);
        }
    }
}
