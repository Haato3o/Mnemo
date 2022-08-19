using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax;
using Mnemo.Core.Syntax.AST;
using Mnemo.Core.Syntax.Stream;
using Mnemo.Core.Utils;
using Mnemo.Tests.Utils;
using System.IO;
using System.Text;

namespace Mnemo.Tests.Core.Syntax.AST
{
    [TestClass]
    public class TestMnemoSyntaxAnalyzer
    {
        [TestMethod]
        public void MnemoSyntaxAnalyzer_ShouldCreateConstDefinitionAST()
        {
            string expression = @"
                const PLAYER_ADDRESS: uint64_t = 0x1412345678;
                
                const getPlayerOffsets(i: int32_t): vector => [
                    0x10,
                    0xA8,
                    0x20 + (i * 8)
                ];
            ";

            TokenStream stream = ToTokenStream(expression);
            var analyzer = new MnemoSyntaxAnalyzer(stream);
            var actual = JsonProvider.Serialize(analyzer.Process());
            
       }

        private static TokenStream ToTokenStream(string expression)
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(expression));
            using StreamReader reader = new(stream);

            var preTokenizer = new MnemoPreTokenizer(reader).Process();
            var lexer = new MnemoLexer(Dependencies.FindAllTokenizers());

            return lexer.Process(preTokenizer);
        }
    }
}
