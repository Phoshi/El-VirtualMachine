namespace Speedycloud.Runtime.Opcodes {
    class JumpIf : IOpcodeHandler {
        private readonly bool condition;

        public JumpIf(bool condition) {
            this.condition = condition;
        }

        public void Accept(Opcode opcode, VirtualMachine machine) {
            var flag = machine.Pop();
            if (flag.Boolean == condition) {
                machine.InstructionPointer += opcode.OpArgs[0];
            }
        }
    }
}
