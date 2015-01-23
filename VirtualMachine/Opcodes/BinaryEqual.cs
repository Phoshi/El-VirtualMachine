namespace Speedycloud.Runtime.Opcodes {
    class BinaryEqual : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var val1 = machine.Pop();
            var val2 = machine.Pop();
            if (val1.Type != val2.Type) {
                machine.Push(machine.ValueFactory.Make(false));
                return;
            }

            machine.Push(machine.ValueFactory.Make(val1.Equals(val2)));
        }
    }
}
