using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class StoreNewName : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var nameId = opcode.OpArgs[0];
            var nameString = machine.Constants[opcode.OpArgs[1]];

            var name = new Name(nameString.String, machine.GetNewNameIdentifier(), StorageType.Heap);

            machine.CurrentNameTable.New(nameId, name);

            machine.Heap[name.Value] = machine.Pop();
        }
    }
}
