using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Syntax;
using Mnemo.Core.Syntax.AST;
using Mnemo.Core.Syntax.AST.Nodes;
using Mnemo.Core.Syntax.Entity;
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
            string expression = "let MUTABLE_VARIABLE: int32_t;";

            TokenStream stream = ToTokenStream(expression);
            var analyzer = new MnemoSyntaxAnalyzer(stream);
            var actual = analyzer.Process();

            var expected = new MnemoRootASTNode
            {
                Name = "Program"
            };
            expected.Nodes.Add(new MnemoDefineMutableASTNode
            {
                Name = new MnemoLiteralASTNode { Value = new BoxedValue() { Value = "MUTABLE_VARIABLE" } },
                Type = new MnemoLiteralASTNode { Value = new BoxedValue() { Value = "int32_t" } },
                Expr = new MnemoASTNode()
            });

            AssertExtensions.AreEqualDeep(actual, expected);
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
