using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class LoadConst : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var constant = machine.Constants[opcode.OpArgs[0]];

            machine.Push(constant);
        }
    }
}
