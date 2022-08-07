using Mnemo.Core.Syntax.Entity;

namespace Mnemo.Core.Syntax.Interfaces
{
    public interface ITokenizer
    {
        public bool CanTokenize(string token);
        public Token Tokenize(string token);
        public object ConvertValue(string token);
    }
}
