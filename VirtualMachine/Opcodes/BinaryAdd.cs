using System.Linq;
using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime.Opcodes {
    class BinaryAdd : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var val2 = machine.Pop();
            var val1 = machine.Pop();

            if (val1.Type == ValueType.Array) {
                var arr = val1.Array.Contents.ToList();
                arr.Add(val2);
                machine.Push(machine.ValueFactory.Make(arr));
            }
            else if (val1.Type == ValueType.String) {
                if (val2.Type == ValueType.String) {
                    machine.Push(machine.ValueFactory.Make(val1.String + val2.String));
                }
                else {
                    machine.Push(machine.ValueFactory.Make(val1.String + (char)val2.Integer));
                }
            }
            else if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    machine.Push(machine.ValueFactory.Make(val1.Integer + val2.Integer));
                }
                else {
                    machine.Push(machine.ValueFactory.Make(val1.Integer + val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    machine.Push(machine.ValueFactory.Make(val1.Double + val2.Integer));
                }
                else {
                    machine.Push(machine.ValueFactory.Make(val1.Double + val2.Double));
                }
            }
        }
    }
}
