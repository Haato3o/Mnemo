namespace Mnemo.Core.Syntax.Entity
{
    internal struct BoxedValue
    {
        public object Value { get; init; }

        public T GetAs<T>()
        {
            return (T)Value;
        }

        public object Get() => Value;
    }
}
