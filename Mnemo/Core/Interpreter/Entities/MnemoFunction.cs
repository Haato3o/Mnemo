using Mnemo.Core.Compiler.Entities;

namespace Mnemo.Core.Interpreter.Entities
{
    internal class MnemoFunction
    {
        private readonly Instructions[] _instructions;
        private readonly MnemoVirtualMachine _vm;

        public MnemoFunction(Instructions[] instructions)
        {
            _instructions = instructions;
        }

        public void Execute(MnemoVirtualMachine vm)
        {

        }
    }
}
