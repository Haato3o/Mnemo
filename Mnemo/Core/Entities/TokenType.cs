namespace Mnemo.Core.Entities
{
    public enum TokenType
    {
        Comment,
        Literal,
        Number,
        Operator,
        Assign,
        BeginScope,
        EndScope,
        Define,
        Callable
    }
}
