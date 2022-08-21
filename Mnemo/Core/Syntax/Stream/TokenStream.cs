using Mnemo.Core.Syntax.Entity;
using System.IO;
using Mnemo.Core.Utils;
using System.Runtime.InteropServices;

namespace Mnemo.Core.Syntax.Stream
{
    internal class TokenStream
    {
        private readonly MemoryStream _stream;
        private readonly static int structSize = Marshal.SizeOf<MnemoToken>();
        private long _rewindPosition = 0;

        public TokenStream(MnemoToken[] tokens)
        {
            _stream = new MemoryStream(tokens.ToBytes());
        }

        public MnemoToken Read()
        {
            return _stream.ReadStructure<MnemoToken>();
        }

        public MnemoToken Peek()
        {
            long position = _stream.Position;
            MnemoToken token = Read();
            _stream.Position = position;
            return token;
        }

        public void Prev() => _stream.Position -= structSize;

        public MnemoToken Next(Token token)
        {
            var mnemoToken = Read();
            
            while (mnemoToken.Token != token)
            {
                mnemoToken = Read();
            }

            return mnemoToken;
        }

        public void Remember()
        {
            _rewindPosition = _stream.Position;
        }

        public void Rewind()
        {
            _stream.Position = _rewindPosition;
        }

        public void Consume()
        {
            _stream.Position += structSize;
        }

        public bool IsEndOfStream() => _stream.Position >= _stream.Length;

        public void Reset() => _stream.Position = 0;

        public MnemoToken[] AsArray()
        {
            long temp = _stream.Position;

            MnemoToken[] tokens = new MnemoToken[_stream.Length / structSize];

            for (int i = 0; i < tokens.Length; i++)
                tokens[i] = _stream.ReadStructure<MnemoToken>();

            _stream.Position = temp;
            return tokens;
        }
    }
}
