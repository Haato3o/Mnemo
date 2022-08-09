using Mnemo.Core.Utils;
using System.IO;

namespace Mnemo.Core.Syntax.Stream
{
    internal class StreamReaderWrapper
    {
        private StreamReader _stream;

        public bool EndOfStream => _stream.EndOfStream;
        
        public long Position { get; private set; }
        public long CurrentLine { get; private set; }

        public StreamReaderWrapper(StreamReader stream)
        {
            _stream = stream;
        }

        public char Peek() => _stream.PeekChar();

        public char Read()
        {
            char character = _stream.ReadChar();

            if (character.IsLineBreak())
                CurrentLine++;

            return character;
        }

        public void Consume() => Read();
    }
}
