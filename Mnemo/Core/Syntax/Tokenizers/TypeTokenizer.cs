using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    internal class TypeTokenizer : ITokenizer
    {
        private readonly HashSet<string> _types = new()
        {
            "uint8_t",
            "uint16_t",
            "uint32_t",
            "uint64_t",
            "int8_t",
            "int16_t",
            "int32_t",
            "int64_t",
            "float",
            "double",
            "vector",
        };

        public bool CanTokenize(string token) => _types.Contains(token);

        public object ConvertValue(string token) => token;

        public Token Tokenize(string token) => Token.Type;
    }
}
