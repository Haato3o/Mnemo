namespace Mnemo.Core.Syntax.Entity
{
    public enum Token
    {
        Type,
        Literal,
        Assign,
        End,
        Intrinsic,
        LT,
        GT,
        ParenStart,
        ParenEnd,
        ListStart,
        ListEnd,
        Value,
        Func,
        Separator,
        Colon,
        DefineConst,
        DefineSoft,
        Import,
        Arithmetic
    }
}
