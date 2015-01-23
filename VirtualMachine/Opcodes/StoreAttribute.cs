namespace Speedycloud.Runtime.Opcodes {
    class StoreAttribute : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var newValue = machine.Pop();
            var complex = machine.Pop();
            machine.Push(complex.Complex.Update(opcode.OpArgs[0], newValue));
        }
    }
}
