using Mnemo.Core.Interpreter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnemo.Core.Interpreter
{
    internal class MnemoVirtualMachine
    {

        private Stack<object> _stack = new Stack<object>();
        private Dictionary<string, MnemoFunction> _functions = new Dictionary<string, MnemoFunction>();

        public MnemoVirtualMachine(Dictionary<string, MnemoFunction> functions)
        {
            _functions = functions;
        }


    }
}
