using System;

namespace Mnemo
{
    public class Mnemo
    {

        public static Mnemo Create(string path, IntPtr pHandle)
        {
            return new Mnemo();
        }
    }
}
