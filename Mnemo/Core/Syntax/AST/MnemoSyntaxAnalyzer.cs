using Mnemo.Core.Syntax.AST.Nodes;
using Mnemo.Core.Syntax.AST.Utils;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnemo.Core.Syntax.AST
{
    internal class MnemoSyntaxAnalyzer
    {
        private readonly TokenStream _stream;
        private readonly MnemoRootASTNode _ast;

        public MnemoSyntaxAnalyzer(TokenStream stream)
        {
            _stream = stream;
        }

        public MnemoRootASTNode Process()
        {



            return _ast;
        }

        MnemoASTNode BuildConstNode()
        {
            var tokenDefine = _stream.Next(Token.DefineConst);
            var tokenName = _stream.Next(Token.DefineConst);
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
                Expr = BuildExpressionNode()
            };

            return node;
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

            return token.Token switch
            {
                Token.ListStart => BuildListNode(),
                _ => throw new NotImplementedException($"TODO: Implement expression for {token.Token}")
            };
        }

        MnemoASTNode BuildFuncNode()
        {
            return null;
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
                    case Token.Arithmetic:
                        {
                            tokens.Enqueue(token);
                            break;
                        }
                    default:
                        throw new NotImplementedException("TODO: Throw better error here");
                        
                }
            }

            return new MnemoArrayASTNode
            {
                Metadata = arrayStarttoken.Metadata,
                Values = nodes.ToArray()
            };
        }
    }
}
