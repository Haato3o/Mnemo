using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mnemo.Tests.Utils
{
    internal static class AssertExtensions
    {
        public static void AreEqualDeep<T>(T actual, T expected)
        {
            Assert.AreEqual(JsonProvider.Serialize(actual), JsonProvider.Serialize(expected));
        }
    }
}
