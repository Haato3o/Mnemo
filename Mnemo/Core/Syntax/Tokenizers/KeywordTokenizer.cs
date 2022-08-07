﻿using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    public class KeywordTokenizer : ITokenizer
    {

        public Dictionary<string, Token> _keywords = new()
        {
            { "const", Token.DefineConst }
        };

        public bool CanTokenize(string token) => _keywords.ContainsKey(token);

        public object ConvertValue(string token) => token;

        public Token Tokenize(string token) => _keywords[token];
    }
}