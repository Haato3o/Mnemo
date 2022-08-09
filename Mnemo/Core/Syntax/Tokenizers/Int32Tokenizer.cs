using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using Mnemo.Core.Syntax.Tokenizers.Entities;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    internal class Int32Tokenizer : ITokenizer
    {
        private readonly char[] validChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private readonly Dictionary<char, int> validCharsLookup = new();

        public Int32Tokenizer()
        {
            for (int i = 0; i < validChars.Length; i++)
                validCharsLookup.Add(validChars[i], i);
        }

        public bool CanTokenize(string token)
        {
            token = token.ToLowerInvariant();

            Int32Type type = Int32Type.Decimal;
            if (token.Length > 2)
                type = token[..2] switch
                {
                    "0b" => Int32Type.Binary,
                    "0o" => Int32Type.Octal,
                    "0x" => Int32Type.Hexadecimal,
                    _ => Int32Type.Decimal
                };

            if (type != Int32Type.Decimal)
                token = token[2..];

            foreach (char c in token)
                if (!validCharsLookup.ContainsKey(c))
                    return false;

            return true;
        }

        public object ConvertValue(string token)
        {
            token = token.ToLowerInvariant();

            Int32Type type = Int32Type.Decimal;
            if (token.Length > 2)
                type = token[..2] switch
                {
                    "0b" => Int32Type.Binary,
                    "0o" => Int32Type.Octal,
                    "0x" => Int32Type.Hexadecimal,
                    _ => Int32Type.Decimal
                };

            if (type != Int32Type.Decimal)
                token = token[2..];

            int value = 0;
            int radix = 1;
            for (int i = token.Length - 1; i >= 0; i--)
            {
                value += validCharsLookup[token[i]] * radix;
                radix *= (int)type;
            }

            return value;
        }

        public Token Tokenize(string token)
        {
            return Token.Value;
        }
    }
}
