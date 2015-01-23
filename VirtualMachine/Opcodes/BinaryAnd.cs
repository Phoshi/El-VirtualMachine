namespace Speedycloud.Runtime.Opcodes {
    class BinaryAnd : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var val1 = machine.Pop();
            var val2 = machine.Pop();
            machine.Push(machine.ValueFactory.Make(val1.Boolean && val2.Boolean));
        }
    }
}
