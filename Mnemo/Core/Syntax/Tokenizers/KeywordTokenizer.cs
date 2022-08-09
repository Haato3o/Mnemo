using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    internal class KeywordTokenizer : ITokenizer
    {

        public Dictionary<string, Token> _keywords = new()
        {
            { "const", Token.DefineConst },
            { "let", Token.DefineSoft },
            { "import", Token.Import }
        };

        public bool CanTokenize(string token) => _keywords.ContainsKey(token);

        public object ConvertValue(string token) => token;

        public Token Tokenize(string token) => _keywords[token];
    }
}
