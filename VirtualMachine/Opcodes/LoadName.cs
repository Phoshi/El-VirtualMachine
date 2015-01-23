using System.Linq;

namespace Speedycloud.Runtime.Opcodes {
    class LoadName : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var nameId = opcode.OpArgs[0];

            var name = machine.CurrentNameTable.Lookup(nameId);
            if (name.Type == StorageType.Heap) {
                machine.Push(machine.Heap[name.Value]);
            }
            else {
                machine.Push(machine.Stack.ElementAt((machine.Stack.Count - machine.CurrentStackFrame) + 2 + name.Value));
            }
        }
    }
}
