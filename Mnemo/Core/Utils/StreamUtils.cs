using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Mnemo.Core.Utils
{
    internal static class StreamUtils
    {
        public static void Consume(this StreamReader self) => self.Read();
        public static char ReadChar(this StreamReader self) => (char)self.Read();
        public static char PeekChar(this StreamReader self) => (char)self.Peek();

        public static T ReadStructure<T>(this MemoryStream self) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            IntPtr malloc = Marshal.AllocHGlobal(typeSize);
            Span<byte> buffer = new byte[typeSize];
            
            self.Read(buffer);
            Marshal.Copy(buffer.ToArray(), 0, malloc, typeSize);

            T structure = Marshal.PtrToStructure<T>(malloc);

            Marshal.FreeHGlobal(malloc);
            return structure;
        }
    }
}
