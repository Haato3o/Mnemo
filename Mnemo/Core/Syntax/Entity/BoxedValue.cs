namespace Mnemo.Core.Syntax.Entity
{
    public struct BoxedValue
    {
        public object Value { get; init; }

        public T GetAs<T>()
        {
            return (T)Value;
        }
    }
}
