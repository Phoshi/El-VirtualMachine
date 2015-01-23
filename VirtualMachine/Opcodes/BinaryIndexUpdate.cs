using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speedycloud.Runtime.Opcodes {
    class BinaryIndexUpdate : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var val = machine.Pop();
            var index = machine.Pop().Integer;
            var array = machine.Pop().Array;

            machine.Push(array.Update(index, val));
        }
    }
}
