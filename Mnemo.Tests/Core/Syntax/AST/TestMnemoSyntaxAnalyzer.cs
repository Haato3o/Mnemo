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
        public void MnemoSyntaxAnalyzer_ShouldCreateLetUndefinedAST()
        {
            string expression = "let MUTABLE_VARIABLE: int32_t;";

            TokenStream stream = ToTokenStream(expression);
            var analyzer = new MnemoSyntaxAnalyzer(stream);
            var actual = analyzer.Process();

            var expected = new MnemoRootASTNode { Name = "Program" };
            expected.Nodes.Add(new MnemoDefineMutableASTNode
            {
                Name = new MnemoLiteralASTNode { Value = new BoxedValue() { Value = "MUTABLE_VARIABLE" } },
                Type = new MnemoLiteralASTNode { Value = new BoxedValue() { Value = "int32_t" } },
                Expr = new MnemoASTNode()
            });

            AssertExtensions.AreEqualDeep(actual, expected);
        }

        [TestMethod]
        public void MnemoSyntaxAnalyzer_ShouldCreateFunctionAST()
        {
            string expression = "const getPlayerLevel(playerIndex: int32_t): int32_t => Read<int32_t>(PLAYER_ADDRESS, [10, 20]);";

            TokenStream stream = ToTokenStream(expression);
            var analyzer = new MnemoSyntaxAnalyzer(stream);
            var actual = analyzer.Process();

            var expected = new MnemoRootASTNode { Name = "Program" };
            expected.Nodes.Add(new MnemoDefineASTNode
            {
                Name = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "getPlayerLevel" } },
                Type = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "int32_t" } },
                Params = new[] { new MnemoParamASTNode { Name = "playerIndex", Type = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "int32_t" } } } },
                Expr = new MnemoFuncASTNode
                {
                    Expression = new MnemoIntrinsicASTNode
                    {
                        Intrinsic = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "Read" } },
                        Generics = new[] { new MnemoLiteralASTNode { Value = new BoxedValue { Value = "int32_t" } } },
                        Parameters = new MnemoASTNode[]
                        {
                            new MnemoVariableASTNode { Name = "PLAYER_ADDRESS" },
                            new MnemoArrayASTNode
                            {
                                Values = new MnemoASTNode[] 
                                {
                                    new MnemoLiteralASTNode { Value = new BoxedValue { Value = 10 } },
                                    new MnemoLiteralASTNode { Value = new BoxedValue { Value = 20 } },
                                }
                            }
                        }
                    }
                }
            });

            AssertExtensions.AreEqualDeep(actual, expected);
        }

        [TestMethod]
        public void MnemoSyntaxAnalyzer_ShouldCreateConstVariableAST()
        {
            string expression = "const test: uint64_t = 0x140000000";

            TokenStream stream = ToTokenStream(expression);
            var analyzer = new MnemoSyntaxAnalyzer(stream);
            var actual = analyzer.Process();

            var expected = new MnemoRootASTNode { Name = "Program" };
            expected.Nodes.Add(new MnemoDefineASTNode
            {
                Name = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "test" } },
                Type = new MnemoLiteralASTNode { Value = new BoxedValue { Value = "uint64_t" } },
                Expr = new MnemoAssignASTNode
                {
                    Expression = new MnemoLiteralASTNode { Value = new BoxedValue { Value = 0x140000000L } }
                }
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
