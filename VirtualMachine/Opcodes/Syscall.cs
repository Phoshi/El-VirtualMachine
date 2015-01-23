using System;

namespace Speedycloud.Runtime.Opcodes {
    class Syscall : IOpcodeHandler{
        public void Accept(Opcode opcode, VirtualMachine machine) {
            switch (opcode.OpArgs[0]) {
                case 0:
                    var character = (char)machine.Pop().Integer;
                    Console.Write(character);
                    break;
                case 1:
                    var chr = Console.Read();
                    machine.Push(machine.ValueFactory.Make(chr));
                    break;
            }
        }
    }
}
