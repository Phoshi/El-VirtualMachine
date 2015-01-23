using System.Collections.Generic;
using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime.Opcodes {
    class MakeRecord : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var slots = new List<IValue>();
            for (int i = 0; i < opcode.OpArgs[0]; i++) {
                slots.Add(machine.Pop());
            }
            slots.Reverse();

            machine.Push(new ComplexValue(slots.ToArray()));
        }
    }
}
