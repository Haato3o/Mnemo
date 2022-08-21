using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mnemo.Core.Enums;
using Mnemo.Core.Syntax;
using Mnemo.Core.Syntax.AST.Nodes;
using Mnemo.Core.Syntax.AST.Utils;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Utils;
using Mnemo.Tests.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mnemo.Tests.Core.Syntax.AST.Utils
{
    [TestClass]
    public class TestMnemoSyntaxHelper
    {

        [TestMethod]
        public void MnemoSyntaxHelper_ShouldConvertInfixQueueToPostfix()
        {
            var expression = "0x20 + index * 8";
            var queue = ToTokenQueue(expression);
            
            var actual = MnemoSyntaxHelper.InfixToPostfix(queue)
                .Select(token => token.Value.Value)
                .ToArray();

            var expected = new object[] { 0x20L, "index", 8L, ArithmeticType.Mul, ArithmeticType.Add };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MnemoSyntaxHelper_ShouldConvertToPostfixWithParenthesis()
        {
            var expression = "(A + B) * (C + (D + E))";
            var queue = ToTokenQueue(expression);

            var actual = MnemoSyntaxHelper.InfixToPostfix(queue)
                .Select(token => token.Value.Value)
                .ToArray();

            var expected = new object[] { "A", "B", ArithmeticType.Add, "C", "D", "E", ArithmeticType.Add, ArithmeticType.Add, ArithmeticType.Mul };

            CollectionAssert.AreEqual(expected, actual);
        }

        /*
            
                    (A + B) * (C + (D + E))

        Should be converted to the following AST representation:

                            ( * )
                          /       \
                       ( + )      ( + )
                       /   \      /   \
                     (+)   (C)   (B) (A)
                    /   \
                  (E)   (D)
        */
        [TestMethod]
        public void MnemoSyntaxHelper_ShouldConvertQueueToASTCorrectly()
        {
            var expression = "(A + B) * (C + (D + E))";
            var queue = ToTokenQueue(expression);
            queue = MnemoSyntaxHelper.InfixToPostfix(queue);

            var actual = queue.ToASTNode();
            var expected = new MnemoArithmeticASTNode()
            {
                Right = new MnemoArithmeticASTNode
                {
                    Right = new MnemoVariableASTNode { Name = "A" },
                    Left = new MnemoVariableASTNode { Name = "B" },
                    Type = ArithmeticType.Add
                },
                Left = new MnemoArithmeticASTNode
                {
                    Right = new MnemoVariableASTNode { Name = "C" },
                    Left = new MnemoArithmeticASTNode
                    {
                        Right = new MnemoVariableASTNode { Name = "D" },
                        Left = new MnemoVariableASTNode { Name = "E" },
                        Type = ArithmeticType.Add
                    },
                    Type = ArithmeticType.Add,
                },
                Type = ArithmeticType.Mul
            };

            AssertExtensions.AreEqualDeep(actual, expected);
        }

        private static Queue<MnemoToken> ToTokenQueue(string expression)
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(expression));
            using StreamReader reader = new(stream);
            
            var preTokenizer = new MnemoPreTokenizer(reader).Process();
            var lexer = new MnemoLexer(Dependencies.FindAllTokenizers());
            var tokens = lexer.Process(preTokenizer).AsArray();

            var queue = new Queue<MnemoToken>(tokens.Length);

            foreach (var token in tokens)
                queue.Enqueue(token);

            return queue;
        }
    }
}
