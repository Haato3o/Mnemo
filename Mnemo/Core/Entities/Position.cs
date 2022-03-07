namespace Mnemo.Core.Entities
{
    public struct Position
    {
        public uint Line { get; init; }
        public uint Character { get; init; }

        public override string ToString()
        {
            return string.Format("({0};{1})", Line, Character);
        }
    }
}
