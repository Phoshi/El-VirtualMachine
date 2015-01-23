namespace Speedycloud.Runtime.Opcodes {
    internal interface IOpcodeHandler {
        void Accept(Opcode opcode, VirtualMachine machine);
    }
}