using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Mnemo.Core.Processing
{
    public class MnemoPreTokenizer
    {

        private StreamReader _stream;
        private readonly List<string> _preTokens = new();

        public MnemoPreTokenizer(StreamReader stream)
        {
            _stream = stream;
        }

        char Read()
        {
            return (char)_stream.Peek();
        }

        char Consume()
        {
            return (char)_stream.Read();
        }

        string ReadString()
        {
            StringBuilder builder = new();

            Stack<char> stack = new();
            stack.Push(Consume());

            while (stack.Count > 0)
            {
                char chr = Consume();

                if (chr == '"')
                {
                    stack.Pop();
                    break;
                }

                builder.Append(chr);
            }

            return builder.ToString();
        }

        string ReadRaw()
        {
            StringBuilder builder = new();
            char[] discard = { ' ', '\n', '\r', '\t' };
            char[] breaks = { '(', ')', '[', ']', ',', ';' };

            char chr = Consume();
            while (!_stream.EndOfStream && !discard.Contains(chr))
            {
                builder.Append(chr);

                if (breaks.Contains(Read()))
                    break;

                chr = Consume();
            }

            return builder.ToString();
        }

        string ReadSingleChar()
        {
            return Consume().ToString();
        }

        string Next()
        {
            char chr = Read();

            return chr switch
            {
                '"' => ReadString(),
                '(' or ')' or '[' or ']' => ReadSingleChar(),
                _ => ReadRaw()
            };
        }

        public string[] PreTokenize()
        {

            while (!_stream.EndOfStream)
            {
                string token = Next();

                if (string.IsNullOrEmpty(token))
                    continue;

                _preTokens.Add(token);
            }

            return _preTokens.ToArray();
        }
    }
}
