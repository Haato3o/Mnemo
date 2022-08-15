namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoCallASTNode : MnemoASTNode
    {
        public MnemoLiteralASTNode Name { get; init; }
        public MnemoASTNode[] Parameters { get; init; }
    }
}
