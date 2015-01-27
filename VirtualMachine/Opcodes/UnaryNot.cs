using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class UnaryNot : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var flag = machine.Pop();
            machine.Push(machine.ValueFactory.Make(!flag.Boolean));
        }
    }
}
