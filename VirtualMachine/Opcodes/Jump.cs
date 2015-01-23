namespace Speedycloud.Runtime.Opcodes {
    class Jump : IOpcodeHandler {
        private readonly bool absolute;

        public Jump(bool absolute = false) {
            this.absolute = absolute;
        }

        public void Accept(Opcode opcode, VirtualMachine machine) {
            if (!absolute)
                machine.InstructionPointer += opcode.OpArgs[0];
            else
                machine.InstructionPointer = opcode.OpArgs[0];
        }
    }
}
