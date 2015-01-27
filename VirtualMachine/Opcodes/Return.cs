using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class Return : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var returnValue = machine.Pop();
            while (machine.Stack.Count > machine.CurrentStackFrame) {
                machine.Stack.Pop();
            }
            var newInstructionPointer = machine.Pop();
            var oldStackFrame = machine.Pop();

            machine.CurrentStackFrame = (int)oldStackFrame.Integer;
            machine.InstructionPointer = (int)newInstructionPointer.Integer;
            machine.CurrentNameTable = machine.CurrentNameTable.Parent;
            machine.Push(returnValue);
        }
    }
}
