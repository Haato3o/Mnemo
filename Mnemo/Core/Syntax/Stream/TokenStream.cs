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

        public TokenStream(MnemoToken[] tokens)
        {
            _stream = new MemoryStream(tokens.ToBytes());
        }

        public MnemoToken Read()
        {
            return _stream.ReadStructure<MnemoToken>();
        }

        public void Consume()
        {
            _stream.Position += structSize;
        }

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
