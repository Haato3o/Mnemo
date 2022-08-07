using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Mnemo.Core.Syntax
{
    public class MnemoLexer
    {
        private readonly ITokenizer[] _tokenizers;
        public MnemoLexer(ITokenizer[] tokenizers)
        {
            _tokenizers = tokenizers;
        }

        public List<MnemoToken> Invoke(string[] preTokens)
        {
            List<MnemoToken> tokens = new(preTokens.Length);

            foreach (string preToken in preTokens)
            {
                MnemoToken token = new MnemoToken()
                {
                    Token = ConvertToToken(preToken),
                    Value = new BoxedValue
                    {
                        Value = ConvertValue(preToken)
                    }
                };

                tokens.Add(token);
            }

            return tokens;
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
