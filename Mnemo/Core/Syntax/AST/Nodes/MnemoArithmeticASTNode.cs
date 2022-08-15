using Mnemo.Core.Enums;

namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoArithmeticASTNode : MnemoASTNode
    {
        public MnemoASTNode Left { get; init; }
        public MnemoASTNode Right { get; init; }
        public ArithmeticType Type { get; init; }
    }
}
