using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class LoadAttribute : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var complex = machine.Pop();
            machine.Push(complex.Complex.Slots[opcode.OpArgs[0]]);
        }
    }
}
