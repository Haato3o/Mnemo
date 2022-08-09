using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;

namespace Mnemo.Core.Syntax.Tokenizers
{
    internal class IntrinsicsTokenizer : ITokenizer
    {
        private HashSet<string> _intrinsics = new()
        {
            { "Read" }
        };

        public bool CanTokenize(string token) => _intrinsics.Contains(token);

        public object ConvertValue(string token) => token;

        public Token Tokenize(string token) => Token.Intrinsic;
    }
}
