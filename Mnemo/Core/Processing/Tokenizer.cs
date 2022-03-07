using Mnemo.Core.Entities;
using Mnemo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnemo.Core.Processing
{
    public class Tokenizer
    {
        private readonly static string[] Operators = { "+", "-", "/", "*" };
        private readonly static Dictionary<string, TokenType> Tokens = new()
        {
            { "{", TokenType.BeginScope },
            { "}", TokenType.EndScope },
            { "=", TokenType.Assign },
        };
        private readonly static Dictionary<string, TokenType> Keywords = new()
        {
            { "let", TokenType.Define },
            { "const", TokenType.Define },
            { "Address", TokenType.Define },
            { "Offsets", TokenType.Define },
        };
        private readonly static string[] NativeCallables = { "Read<Int32>", "Read<Int64>", "Read<Float>", "Read<Double>" };
        private readonly PreToken[] _preTokens;
        private readonly Token[] _tokens;

        public Tokenizer(PreToken[] preTokens)
        {
            _preTokens = preTokens;
            _tokens = new Token[preTokens.Length];
        }

        public Token[] Tokenize()
        {
            for (int i = 0; i < _preTokens.Length; i++)
            {
                ref PreToken preToken = ref _preTokens[i];
               _tokens[i] = DetectType(preToken);
            }
            return _tokens;
        }

        private Token DetectType(PreToken preToken)
        {
            string literal = preToken.Literal;
            object boxedValue = literal;

            TokenType type;
            if (literal.StartsWith("//"))
                type = TokenType.Comment;
            else if (Operators.Contains(literal))
            {
                type = TokenType.Operator;
                boxedValue = literal switch
                {
                    "+" => OperatorType.Add,
                    "-" => OperatorType.Sub,
                    "*" => OperatorType.Mul,
                    "/" => OperatorType.Div,
                    _ => throw new NotImplementedException("unreachable")
                };
            }
            else if (TryParseNumber(literal) is long value)
            {
                type = TokenType.Number;
                boxedValue = value;
            }
            else if (Tokens.ContainsKey(literal))
                type = Tokens[literal];
            else if (NativeCallables.Contains(literal))
                type = TokenType.Callable;
            else if (Keywords.ContainsKey(literal))
                type = Keywords[literal];
            else if (literal.StartsWith("\""))
                type = TokenType.String;
            else
                type = TokenType.Literal;


            Debug.Assert(type != TokenType.Invalid);

            return new Token()
            {
                Literal = preToken.Literal,
                Position = preToken.Position,
                Box = boxedValue,
                Type = type
            };
        }

        private long? TryParseNumber(string literal)
        {
            NumberStyles prefix = literal.Length > 2 && literal.Substring(0, 2).ToLowerInvariant() == "0x"
                    ? NumberStyles.HexNumber
                    : NumberStyles.Integer;
            
            if (prefix == NumberStyles.HexNumber)
                literal = literal.Substring(2, literal.Length - 2);

            bool success = long.TryParse(literal, prefix, null, out long result);

            return success ? result : null;
        }
    }
}
