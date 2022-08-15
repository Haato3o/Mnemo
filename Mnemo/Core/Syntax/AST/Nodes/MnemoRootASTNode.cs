using System.Collections.Generic;

namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoRootASTNode : MnemoASTNode
    {
        public string Name { get; init; }

        public List<MnemoASTNode> Nodes { get; } = new();
    }
}
