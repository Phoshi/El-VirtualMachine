namespace Speedycloud.Runtime.Opcodes {
    class BinaryIndex : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var index = machine.Pop();
            var array = machine.Pop();
            machine.Push(array.Array.Contents[(int)index.Integer]);
        }
    }
}
