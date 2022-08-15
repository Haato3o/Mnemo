namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoDefineASTNode : MnemoASTNode
    {
        public MnemoLiteralASTNode Name { get; init; }
        public MnemoLiteralASTNode Type { get; init; }
        
        public MnemoASTNode Expr { get; init; }
    }
}
