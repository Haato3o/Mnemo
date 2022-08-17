using Mnemo.Core.Enums;
using Mnemo.Core.Syntax.AST.Nodes;
using Mnemo.Core.Syntax.Entity;
using System;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.AST.Utils
{
    internal static class MnemoSyntaxHelper
    {
        private static Dictionary<object, int> _precedences = new()
        {
            { ArithmeticType.Mul, 3 },
            { ArithmeticType.Div, 3 },
            { ArithmeticType.Add, 2 },
            { ArithmeticType.Sub, 2 },
            { "(", 1 },
            { ")", 1 },
        };

        private static bool IsHigherPrecedence(object left, object right)
        {
            return _precedences[left] >= _precedences[right];
        }

        public static Queue<MnemoToken> InfixToPostfix(Queue<MnemoToken> tokens)
        {
            Queue<MnemoToken> operands = new();
            Stack<MnemoToken> operators = new();

            while (tokens.Count > 0)
            {
                MnemoToken token = tokens.Dequeue();
                
                if (token.Token == Token.Arithmetic)
                {
                    if (operators.TryPeek(out MnemoToken peek))
                    {
                        if (IsHigherPrecedence(token.Value.Get(), peek.Value.Get()))
                        {
                            operators.Push(token);
                            continue;
                        }

                        while (!IsHigherPrecedence(token.Value.Get(), peek.Value.Get()))
                        {
                            operands.Enqueue(operators.Pop());
                            peek = operands.Peek();
                        }
                    } else
                    {
                        operators.Push(token);
                    }

                    continue;
                }

                if (token.Token == Token.ParenEnd)
                {
                    while (operators.TryPeek(out MnemoToken peek))
                    {
                        var popped = operators.Pop();

                        if (peek.Token == Token.ParenStart)
                            break;
                        
                        operands.Enqueue(popped);

                    }
                    continue;
                }

                if (token.Token == Token.ParenStart)
                {
                    operators.Push(token);
                    continue;
                }

                operands.Enqueue(token);
            }

            while (operators.Count > 0)
                operands.Enqueue(operators.Pop());

            return operands;
        }

        public static MnemoASTNode ToASTNode(this Queue<MnemoToken> tokens)
        {
            if (tokens.Count == 1)
            {
                var token = tokens.Dequeue();
                return token.Token switch
                {
                    Token.Literal => new MnemoVariableASTNode() { Name = token.Value.GetAs<string>(), Metadata = token.Metadata },
                    Token.Value => new MnemoLiteralASTNode() { Value = token.Value, Metadata = token.Metadata },
                    _ => throw new NotImplementedException()
                };
            }

            Stack<MnemoASTNode> stack = new Stack<MnemoASTNode>();

            while (tokens.Count > 0)
            {
                var token = tokens.Dequeue();

                switch (token.Token)
                {
                    case Token.Arithmetic:
                        {
                            var node = new MnemoArithmeticASTNode()
                            {
                                Metadata = token.Metadata,
                                Left = stack.Pop(),
                                Right = stack.Pop(),
                                Type = token.Value.GetAs<ArithmeticType>()
                            };

                            stack.Push(node);
                            break;
                        }
                    case Token.Literal:
                        {
                            var node = new MnemoVariableASTNode()
                            {
                                Name = token.Value.GetAs<string>(),
                                Metadata = token.Metadata
                            };

                            stack.Push(node);
                            break;
                        }
                    case Token.Value:
                        {
                            var node = new MnemoLiteralASTNode()
                            {
                                Value = token.Value,
                                Metadata = token.Metadata
                            };

                            stack.Push(node);

                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }

            return stack.Pop();
        }
    }
}
