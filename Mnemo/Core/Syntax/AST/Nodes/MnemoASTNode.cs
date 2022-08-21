using Mnemo.Core.Syntax.Entity;
using Newtonsoft.Json;

namespace Mnemo.Core.Syntax.AST.Nodes
{
    internal class MnemoASTNode
    {
        [JsonIgnore]
        public TokenMetadata Metadata { get; init; }
    }
}
