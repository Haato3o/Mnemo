using System.IO;

namespace Mnemo.Core.Utils
{
    public static class StreamUtils
    {
        public static void Consume(this StreamReader self) => self.Read();
        public static char ReadChar(this StreamReader self) => (char)self.Read();
        public static char PeekChar(this StreamReader self) => (char)self.Peek();
    }
}
