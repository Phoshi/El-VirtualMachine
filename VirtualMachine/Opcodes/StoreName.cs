using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class StoreName : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var nameId = opcode.OpArgs[0];
            

            var name = machine.CurrentNameTable.Lookup(nameId);

            var newName = new Name(name.String, machine.GetNewNameIdentifier(), StorageType.Heap);
            machine.CurrentNameTable.Update(nameId, newName);

            machine.Heap[newName.Value] = machine.Pop();
        }
    }
}
