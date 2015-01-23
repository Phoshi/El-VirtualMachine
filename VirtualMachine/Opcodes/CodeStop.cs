namespace Speedycloud.Runtime.Opcodes {
    class CodeStop : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            machine.ExecutionBegun = false;
        }
    }
}
