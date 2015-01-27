using Speedycloud.Bytecode;
using Speedycloud.Bytecode.ValueTypes;
using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime.Opcodes {
    class BinaryLessThanOrEqual :IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var val2 = machine.Pop();
            var val1 = machine.Pop();

            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    machine.Push(machine.ValueFactory.Make(val1.Integer <= val2.Integer));
                }
                else {
                    machine.Push(machine.ValueFactory.Make(val1.Integer <= val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    machine.Push(machine.ValueFactory.Make(val1.Double <= val2.Integer));
                }
                else {
                    machine.Push(machine.ValueFactory.Make(val1.Double <= val2.Double));
                }
            }
        }
    }
}
