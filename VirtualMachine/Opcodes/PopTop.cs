using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class PopTop : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            machine.Stack.Pop();
        }
    }
}
