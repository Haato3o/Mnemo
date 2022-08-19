namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoParamASTNode : MnemoASTNode
    {
        public string Name { get; init; }
        public MnemoLiteralASTNode Type { get; init; }
    }
}
