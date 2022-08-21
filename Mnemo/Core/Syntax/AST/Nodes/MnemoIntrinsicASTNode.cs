namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoIntrinsicASTNode : MnemoASTNode
    {
        public MnemoLiteralASTNode Intrinsic { get; init; }
        public MnemoLiteralASTNode[] Generics { get; init; }
        public MnemoASTNode[] Parameters { get; init; }
    }
}
