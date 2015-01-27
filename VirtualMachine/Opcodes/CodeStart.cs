using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class CodeStart : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            if (machine.ExecutionBegun)
                throw new RuntimeException("CODE_START found while code is executing");
            machine.ExecutionBegun = true;
        }
    }
}
