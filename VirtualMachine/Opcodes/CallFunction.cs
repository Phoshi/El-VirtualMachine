using Speedycloud.Bytecode;

namespace Speedycloud.Runtime.Opcodes {
    class CallFunction : IOpcodeHandler {
        public void Accept(Opcode opcode, VirtualMachine machine) {
            var target = machine.Constants[opcode.OpArgs[0]];
            var paramCount = opcode.OpArgs[1];
            machine.Push(machine.ValueFactory.Make(machine.CurrentStackFrame));
            machine.Push(machine.ValueFactory.Make(machine.InstructionPointer));
            machine.Push(machine.ValueFactory.Make(paramCount));

            machine.CurrentStackFrame = machine.Stack.Count;
            machine.InstructionPointer = (int)target.Integer;

            machine.CurrentNameTable = new NameTable(machine.CurrentNameTable);
            for (int i = 0; i < paramCount; i++) {
                machine.CurrentNameTable.New(i, new Name("param", i, StorageType.Stack));
            }
        }
    }
}
