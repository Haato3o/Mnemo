namespace Mnemo.Core.Entities
{
    public struct Token
    {
        public string Literal { get; init; }
        public Position Position { get; init; }
        public TokenType Type { get; init; }
    }
}
