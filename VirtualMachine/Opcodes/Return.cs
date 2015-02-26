using System.Linq;
using Speedycloud.Bytecode;
using Speedycloud.Bytecode.ValueTypes;

namespace Speedycloud.Runtime.Opcodes {
    class Return : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            IValue returnValue = null;
            if (opcode.OpArgs.Any()) {
                returnValue = machine.Pop();
            }
            while (machine.Stack.Count > machine.CurrentStackFrame) {
                machine.Stack.Pop();
            }
            var newInstructionPointer = machine.Pop();
            var oldStackFrame = machine.Pop();

            machine.CurrentStackFrame = (int)oldStackFrame.Integer;
            machine.InstructionPointer = (int)newInstructionPointer.Integer;
            machine.CurrentNameTable = machine.CurrentNameTable.Parent;
            if (returnValue != null) {
                machine.Push(returnValue);
            }
        }
    }
}
