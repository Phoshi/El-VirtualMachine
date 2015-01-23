namespace Speedycloud.Runtime.Opcodes {
    class BinaryNotEqual : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            new BinaryEqual().Accept(opcode, machine);
            machine.Push(machine.ValueFactory.Make(!machine.Pop().Boolean));
        }
    }
}
