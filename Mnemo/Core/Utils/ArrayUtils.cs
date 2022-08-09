using System;
using System.Runtime.InteropServices;

namespace Mnemo.Core.Utils
{
    internal static class ArrayUtils
    {
        public static byte[] ToBytes<T>(this T[] values) where T : struct
        {
            int structSize = Marshal.SizeOf<T>();
            IntPtr unmanaged = Marshal.AllocHGlobal(structSize * values.Length);
            
            for (int i = 0; i < values.Length; i++)
                Marshal.StructureToPtr(values[i], unmanaged + (i * structSize), false);

            byte[] buffer = new byte[values.Length * Marshal.SizeOf<T>()];

            Marshal.Copy(unmanaged, buffer, 0, buffer.Length);

            Marshal.FreeHGlobal(unmanaged);

            return buffer;
        }
    }
}
