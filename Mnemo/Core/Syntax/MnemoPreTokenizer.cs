using Mnemo.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnemo.Core.Syntax
{
    public class MnemoPreTokenizer
    {
        public readonly StreamReader _stream;

        public MnemoPreTokenizer(StreamReader stream)
        {
            _stream = stream;
        }

        public string[] Process()
        {
            List<string> preTokens = new();

            while (!_stream.EndOfStream)
            {
                char currentChar = _stream.ReadChar();

                if (currentChar.IsSpace() || currentChar.IsInvalid())
                    continue;

                char nextChar = _stream.PeekChar();

                string preToken = Read(currentChar, nextChar);

                if (preToken.Length == 0)
                    continue;

                preTokens.Add(preToken);
            }

            return preTokens.ToArray();
        }

        private string Read(char current, char next)
        {
            return current switch
            {
                '+' or '-' or '/' or '*'
                or '(' or ')' or '[' or ']'
                or '<' or '>' or ';' or ','
                or ':' => current.ToString(),
                '=' => ReadArrow(current, next),
                _ => ReadLiteral(current)
            };
        }

        private string ReadLiteral(char current)
        {
            StringBuilder builder = new();
            while (!_stream.EndOfStream && current.IsPartOfLiteral())
            {
                builder.Append(current);

                if (!_stream.PeekChar().IsPartOfLiteral())
                    break;

                current = _stream.ReadChar();
            }

            return builder.ToString();
        }

        private string ReadArrow(char current, char next)
        {
            if (next == '>')
            {
                _stream.Consume();
                return "=>";
            }

            return current.ToString();
        }
    }
}
