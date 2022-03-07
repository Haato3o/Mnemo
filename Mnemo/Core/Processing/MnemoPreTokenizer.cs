using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Mnemo.Core.Entities;

namespace Mnemo.Core.Processing
{
    public class MnemoPreTokenizer
    {

        private StreamReader _stream;
        private readonly List<PreToken> _preTokens = new();
        private uint _lineCount = 0;
        private uint _caret = 0;

        public MnemoPreTokenizer(StreamReader stream)
        {
            _stream = stream;
        }

        private char Read()
        {
            _caret++;
            return (char)_stream.Peek();
        }

        private char Consume()
        {
            return (char)_stream.Read();
        }

        private string ReadString()
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

        private string ReadRaw()
        {
            StringBuilder builder = new();
            char[] discard = { ' ', '\n', '\r', '\t' };
            char[] breaks = { '(', ')', '[', ']', ',', ';' };

            char chr = Consume();

            if (chr == '\n')
                _lineCount++;

            while (!_stream.EndOfStream && !discard.Contains(chr))
            {
                builder.Append(chr);

                if (breaks.Contains(Read()))
                    break;

                chr = Consume();
            }

            return builder.ToString();
        }

        private string ReadSingleChar()
        {
            return Consume().ToString();
        }

        private PreToken Next()
        {
            char chr = Read();
            Position position = new()
            {
                Line = _lineCount,
                Character = _caret
            };

            string literal = chr switch
            {
                '"' => ReadString(),
                '(' or ')' or '[' or ']' => ReadSingleChar(),
                _ => ReadRaw()
            };

            return new PreToken()
            {
                Literal = literal,
                Position = position
            };
        }

        public PreToken[] PreTokenize()
        {

            while (!_stream.EndOfStream)
            {
                PreToken token = Next();

                if (string.IsNullOrEmpty(token.Literal))
                    continue;

                _preTokens.Add(token);
            }

            return _preTokens.ToArray();
        }
    }
}
