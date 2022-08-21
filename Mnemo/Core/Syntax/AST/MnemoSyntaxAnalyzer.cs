using Mnemo.Core.Syntax.AST.Nodes;
using Mnemo.Core.Syntax.AST.Utils;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Stream;
using System;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.AST
{
    internal class MnemoSyntaxAnalyzer
    {
        private readonly TokenStream _stream;

        public MnemoSyntaxAnalyzer(TokenStream stream)
        {
            _stream = stream;
        }

        public MnemoRootASTNode Process()
        {
            MnemoRootASTNode root = new MnemoRootASTNode()
            {
                Name = "Program"
            };

            while (!_stream.IsEndOfStream())
            {
                var token = _stream.Peek();

                if (token.Token == Token.End)
                {
                    _stream.Consume();
                    continue;
                }

                MnemoASTNode node = token.Token switch
                {
                    Token.DefineConst => BuildConstNode(),
                    Token.DefineSoft => BuildLetNode(),
                    _ => throw new NotImplementedException("TODO: Implement other tokens")
                };

                root.Nodes.Add(node);
            }

            return root;
        }

        MnemoASTNode BuildLetNode()
        {
            var tokenDefine = _stream.Next(Token.DefineSoft);
            var tokenName = _stream.Next(Token.Literal);
            var tokenType = _stream.Next(Token.Type);
            var tokenEnd = _stream.Peek();

            return new MnemoDefineMutableASTNode
            {
                Metadata = tokenDefine.Metadata,
                Name = new MnemoLiteralASTNode
                {
                    Metadata = tokenName.Metadata,
                    Value = tokenName.Value,
                },
                Type = new MnemoLiteralASTNode
                {
                    Metadata = tokenType.Metadata,
                    Value = tokenType.Value
                },
                Expr = tokenEnd.Token == Token.End 
                        ? new MnemoASTNode { Metadata = tokenEnd.Metadata } 
                        : BuildAssignNode(),
            };
        }

        MnemoASTNode BuildConstNode()
        {
            var tokenDefine = _stream.Next(Token.DefineConst);
            var tokenName = _stream.Next(Token.Literal);
            MnemoParamASTNode[] parameters = Array.Empty<MnemoParamASTNode>();

            if (_stream.Peek().Token == Token.ParenStart)
            {
                parameters = BuildParamsNodes().ToArray();
            }

            var tokenType = _stream.Next(Token.Type);

            MnemoDefineASTNode node = new MnemoDefineASTNode()
            {
                Metadata = tokenDefine.Metadata,
                
                Name = new()
                {
                    Value = tokenName.Value,
                    Metadata = tokenName.Metadata
                },
                Type = new()
                {
                    Value = tokenType.Value,
                    Metadata = tokenType.Metadata,
                },
                Params = parameters,
                Expr = BuildExpressionNode()
            };

            return node;
        }

        List<MnemoParamASTNode> BuildParamsNodes()
        {
            List<MnemoParamASTNode> nodes = new List<MnemoParamASTNode>();

            MnemoToken token = _stream.Read();
            while (token.Token != Token.ParenEnd)
            {
                if (token.Token == Token.ParenStart ||
                    token.Token == Token.Separator)
                {
                    token = _stream.Read();
                    continue;
                }

                MnemoToken paramType = _stream.Next(Token.Type);

                nodes.Add(new MnemoParamASTNode()
                {
                    Metadata = token.Metadata,
                    Type = new MnemoLiteralASTNode
                    {
                        Metadata = paramType.Metadata,
                        Value = paramType.Value
                    },
                    Name = token.Value.GetAs<string>()
                });

                token = _stream.Read();
            }

            return nodes;
        }

        MnemoASTNode BuildExpressionNode()
        {
            var token = _stream.Peek();

            return token.Token switch
            {
                Token.End => new() { Metadata = token.Metadata },
                Token.Assign => BuildAssignNode(),
                Token.Func => BuildFuncNode(),
                _ => throw new NotImplementedException()
            };
        }

        MnemoASTNode BuildAssignNode()
        {
            var assignToken = _stream.Read();
            var token = _stream.Peek();

            MnemoASTNode expr = token.Token switch
            {
                Token.ListStart => BuildListNode(),
                Token.Value => BuildLiteralNode(),
                _ => throw new NotImplementedException($"TODO: Implement expression for {token.Token}")
            };

            return new MnemoAssignASTNode()
            {
                Metadata = assignToken.Metadata,
                Expression = expr
            };
        }

        MnemoLiteralASTNode BuildLiteralNode()
        {
            var token = _stream.Read();

            return new MnemoLiteralASTNode()
            {
                Metadata = token.Metadata,
                Value = token.Value
            };
        }

        MnemoASTNode BuildFuncNode()
        {
            var funcToken = _stream.Read();
            var token = _stream.Peek();

            MnemoASTNode expr = token.Token switch
            {
                Token.ListStart => BuildListNode(),
                Token.Intrinsic => BuildIntrinsicNode(),
                _ => throw new NotImplementedException($"Not implemented expression for {token.Token}")
            };

            return new MnemoFuncASTNode()
            {
                Metadata = funcToken.Metadata,
                Expression = expr
            };
        }

        MnemoASTNode BuildIntrinsicNode()
        {
            var intrinsicNode = _stream.Read();


            return new MnemoIntrinsicASTNode()
            {
                Intrinsic = new MnemoLiteralASTNode
                {
                    Metadata = intrinsicNode.Metadata,
                    Value = intrinsicNode.Value
                },
                Generics = BuildGenericTypeNodes(),
                Parameters = BuildArgumentNodes()
            };
        }

        MnemoASTNode[] BuildArgumentNodes()
        {
            if (_stream.Peek().Token != Token.ParenStart)
                throw new Exception("TODO: Better exception message for invalid syntax");

            List<MnemoASTNode> nodes = new();
            var token = _stream.Read();
            while (token.Token != Token.ParenEnd)
            {
                switch (token.Token) {
                    case Token.Literal:
                        {
                            var nextToken = _stream.Peek();
                            MnemoASTNode node = nextToken.Token switch
                            {
                                Token.ParenStart => new MnemoCallASTNode
                                {
                                    Metadata = token.Metadata,
                                    Name = new MnemoLiteralASTNode
                                    {
                                        Metadata = token.Metadata,
                                        Value = token.Value,
                                    },
                                    Parameters = BuildArgumentNodes(),
                                },
                                _ => new MnemoVariableASTNode
                                {
                                    Metadata = token.Metadata,
                                    Name = token.Value.GetAs<string>()
                                },
                            };

                            nodes.Add(node);

                            break;
                        }
                    case Token.Value:
                        {
                            nodes.Add(new MnemoLiteralASTNode
                            {
                                Value = token.Value,
                                Metadata = token.Metadata
                            });
                            break;
                        }
                    case Token.ParenStart:
                    case Token.Separator:
                        break;
                    default:
                        throw new Exception("TODO: Better exception for invalid argument");
                }

                token = _stream.Read();
            }

            return nodes.ToArray();
        }

        MnemoLiteralASTNode[] BuildGenericTypeNodes()
        {
            if (_stream.Peek().Token != Token.LT)
                return Array.Empty<MnemoLiteralASTNode>();

            List<MnemoLiteralASTNode> nodes = new();
            MnemoToken token = _stream.Read();
            
            while (token.Token != Token.GT)
            {
                if (token.Token == Token.Type)
                {
                    nodes.Add(new MnemoLiteralASTNode
                    {
                        Value = token.Value,
                        Metadata = token.Metadata
                    });
                }

                token = _stream.Read();
            }

            return nodes.ToArray();
        }

        MnemoASTNode BuildListNode()
        {
            List<MnemoASTNode> nodes = new();
            Queue<MnemoToken> tokens = new();

            var arrayStarttoken = _stream.Read();
            var token = _stream.Read();

            while (token.Token != Token.ListEnd)
            {
                
                switch (token.Token)
                {
                    case Token.Separator:
                        {
                            Queue<MnemoToken> postfixTokens = MnemoSyntaxHelper.InfixToPostfix(tokens);

                            nodes.Add(postfixTokens.ToASTNode());

                            tokens.Clear();
                            break;
                        }
                    case Token.Literal:
                    case Token.Value:
                    case Token.ParenStart:
                    case Token.ParenEnd:
                    case Token.Arithmetic:
                        {
                            tokens.Enqueue(token);
                            break;
                        }
                    default:
                        throw new NotImplementedException("TODO: Throw better error here");
                        
                }

                token = _stream.Read();
            }

            if (tokens.Count > 0)
                nodes.Add(
                    MnemoSyntaxHelper.InfixToPostfix(tokens).ToASTNode()
                );

            return new MnemoArrayASTNode
            {
                Metadata = arrayStarttoken.Metadata,
                Values = nodes.ToArray()
            };
        }
    }
}
