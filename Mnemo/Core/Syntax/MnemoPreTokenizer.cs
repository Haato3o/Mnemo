using Mnemo.Core.Syntax.Entity;
using Mnemo.Core.Syntax.Stream;
using Mnemo.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mnemo.Core.Syntax
{
    internal class MnemoPreTokenizer
    {
        public readonly StreamReaderWrapper _stream;

        public MnemoPreTokenizer(StreamReader stream)
        {
            _stream = new StreamReaderWrapper(stream);
        }

        public PreToken[] Process()
        {
            List<PreToken> preTokens = new();

            while (!_stream.EndOfStream)
            {
                char currentChar = _stream.Read();

                if (currentChar.IsSpace() || currentChar.IsInvalid())
                    continue;

                char nextChar = _stream.Peek();

                string preToken = Read(currentChar, nextChar);

                if (preToken.Length == 0)
                    continue;

                PreToken entity = new PreToken
                {
                    Metadata = new TokenMetadata
                    {
                        // TODO: Add file name
                        Line = _stream.CurrentLine,
                        Position = _stream.Position
                    },
                    Value = preToken
                };

                preTokens.Add(entity);
            }

            return preTokens.ToArray();
        }

        private string Read(char current, char next)
        {
            return current switch
            {
                '+' or '-' or '*'
                or '(' or ')' or '[' or ']'
                or '<' or '>' or ';' or ','
                or ':' => current.ToString(),
                '=' => ReadArrow(current, next),
                '"' => ReadString(current),
                '/' => ReadComment(current, next),
                _ => ReadLiteral(current)
            };
        }

        private string ReadComment(char current, char next)
        {
            if (current != next)
                return current.ToString();

            while (!_stream.EndOfStream && !current.IsEnd())
            {
                current = _stream.Read();
            }

            return string.Empty;
        }

        private string ReadLiteral(char current)
        {
            StringBuilder builder = new();
            while (!_stream.EndOfStream && current.IsPartOfLiteral())
            {
                builder.Append(current);

                if (!_stream.Peek().IsPartOfLiteral())
                    break;

                current = _stream.Read();
            }

            if (builder.Length == 0 && current.IsPartOfLiteral())
                builder.Append(current);

            return builder.ToString();
        }

        private string ReadString(char current)
        {
            StringBuilder builder = new();
            do
            {
                builder.Append(current);

                current = _stream.Read();

            } while (!_stream.EndOfStream && !current.IsDoubleQuote());

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
