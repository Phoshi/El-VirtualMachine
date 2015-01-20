using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine {
    class VirtualMachine {
        private readonly Dictionary<int, IValue> Constants;

        private Dictionary<int, IValue> Heap = new Dictionary<int, IValue>();
 
        private Stack<IValue> Stack = new Stack<IValue>();
        private int currentStackFrame = 0;

        private NameTable currentNameTable = new NameTable();

        private readonly List<Opcode> InstructionStream;
        private int instructionPointer = 0;

        private bool executionBegun = false;

        private Dictionary<Instruction, Action<Opcode>> handlers;

        private Opcode GetNextOpcode() {
            return InstructionStream[instructionPointer++];
        }

        private Opcode GetCurrentOpcode() {
            return InstructionStream[instructionPointer];
        }

        public VirtualMachine(List<Opcode> instructionStream, Dictionary<int, IValue> constants) {
            this.InstructionStream = instructionStream;
            this.Constants = constants;

            initialiseHandlers();
        }

        private void initialiseHandlers() {
            handlers = new Dictionary<Instruction, Action<Opcode>> {
                {Instruction.CODE_START, CodeStart}
            }; 
        }

        public void Run() {
            while (true) {
                Step();
                if (GetCurrentOpcode().Instruction == Instruction.CODE_STOP) {
                    return;
                }
            }
        }

        public void Step() {
            var opcode = GetNextOpcode();
            if (!executionBegun && opcode.Instruction != Instruction.CODE_START) {
                return;
            }

            handlers[opcode.Instruction](opcode);
        }

        private void CodeStart(Opcode opcode) {
            if (executionBegun)
                throw new RuntimeException("CODE_START found while code is executing");
            executionBegun = true;
        }
    }
}
