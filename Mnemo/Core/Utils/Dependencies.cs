using Mnemo.Core.Syntax.Interfaces;
using System;
using System.Linq;

namespace Mnemo.Core.Utils
{
    internal static class Dependencies
    {
        public static ITokenizer[] FindAllTokenizers()
        {
            var implementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.GetInterface(nameof(ITokenizer)) is not null);

            return implementations.Select(type => (ITokenizer)Activator.CreateInstance(type))
                .ToArray();
        }
    }
}
