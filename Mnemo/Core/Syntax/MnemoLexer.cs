using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using Mnemo.Core.Syntax.Stream;
using System.Collections.Generic;
using System.Linq;

namespace Mnemo.Core.Syntax
{
    internal class MnemoLexer
    {
        private readonly ITokenizer[] _tokenizers;
        public MnemoLexer(ITokenizer[] tokenizers)
        {
            _tokenizers = tokenizers;
        }

        public TokenStream Process(PreToken[] preTokens)
        {
            List<MnemoToken> tokens = new(preTokens.Length);

            foreach (PreToken preToken in preTokens)
            {
                MnemoToken token = new MnemoToken()
                {
                    Metadata = preToken.Metadata,
                    Token = ConvertToToken(preToken.Value),
                    Value = new BoxedValue
                    {
                        Value = ConvertValue(preToken.Value)
                    }
                };

                tokens.Add(token);
            }

            return new TokenStream(tokens.ToArray());
        }

        private Token ConvertToToken(string preToken)
        {
            return _tokenizers.FirstOrDefault(t => t.CanTokenize(preToken))?
                              .Tokenize(preToken) ?? Token.Literal;
        }

        private object ConvertValue(string preToken)
        {
            return _tokenizers.FirstOrDefault(t => t.CanTokenize(preToken))?
                              .ConvertValue(preToken) ?? preToken;
        }
    }
}
