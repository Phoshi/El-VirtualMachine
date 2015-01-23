using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime.Opcodes {
    class UnaryNegative :IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var number = machine.Pop();
            if (number.Type == ValueType.Integer) {
                machine.Push(machine.ValueFactory.Make(-number.Integer));
            }
            else {
                machine.Push(machine.ValueFactory.Make(-number.Double));
            }
        }
    }
}
