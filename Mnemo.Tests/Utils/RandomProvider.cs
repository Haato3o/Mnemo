using PeanutButter.RandomGenerators;

namespace Mnemo.Tests.Utils
{
    public static class RandomProvider
    {
        public static T[] Random<T>(int count)
        {
            T[] values = new T[count];
            UniqueRandomValueGenerator<T> generator = new UniqueRandomValueGenerator<T>();

            for (int i = 0; i < count; i++)
                values[i] = generator.Next();

            return values;
        }

        public static T Random<T>()
        {
            UniqueRandomValueGenerator<T> generator = new UniqueRandomValueGenerator<T>();
            return generator.Next();
        }
    }
}
