using Mnemo.Core.Enums;
using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    internal class SimpleCharTokenizers : ITokenizer
    {
        private readonly Dictionary<string, Token> _lookup = new()
        {
            { "(", Token.ParenStart },
            { ")", Token.ParenEnd },
            { "<", Token.LT },
            { ">", Token.GT },
            { "[", Token.ListStart },
            { "]", Token.ListEnd },
            { "=", Token.Assign },
            { "=>", Token.Func },
            { ",", Token.Separator },
            { ":", Token.Colon },
            { "+", Token.Arithmetic },
            { "-", Token.Arithmetic },
            { "*", Token.Arithmetic },
            { "/", Token.Arithmetic },
            { ";", Token.End },
        };

        private readonly Dictionary<string, object> _conversion = new()
        {
            { "+", ArithmeticType.Add },
            { "-", ArithmeticType.Sub },
            { "*", ArithmeticType.Mul },
            { "/", ArithmeticType.Div },
        };

        public bool CanTokenize(string token) => _lookup.ContainsKey(token);

        public object ConvertValue(string token)
        {
            if (_conversion.TryGetValue(token, out object result))
                return result;

            return token;
        }

        public Token Tokenize(string token) => _lookup[token];
    }
}
