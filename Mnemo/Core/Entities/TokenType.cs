namespace Mnemo.Core.Entities
{
    public enum TokenType
    {
        Invalid,
        Comment,
        Literal,
        Number,
        String,
        Operator,
        Assign,
        BeginScope,
        EndScope,
        Define,
        Callable
    }
}
