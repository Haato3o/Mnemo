using Mnemo.Core.Syntax.AST.Nodes;
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
            var token = _stream.Read();

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
            var token = _stream.Read();
            
            if (token.Token == Token.ListStart)
            {

            }

            return null;
        }

        MnemoASTNode BuildFuncNode()
        {
            return null;
        }

        MnemoASTNode BuildListNode()
        {
            List<MnemoLiteralASTNode> nodes = new();
            Queue<MnemoToken> tokens = new();
            var token = _stream.Read();

            while (token.Token != Token.ListEnd)
            {
                
                if (token.Token == Token.Separator)
                {

                    continue;
                }
                    

                tokens.Enqueue(token);

                if (token.Token == Token.Value)
                    nodes.Add(new MnemoLiteralASTNode()
                    {
                        Metadata = token.Metadata,
                        Value = token.Value
                    });
                else if (token.Token == Token.Arithmetic)
                    return null;
            }

            return null;
        }
    }
}
