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
        private void initialiseHandlers() {
            handlers = new Dictionary<Instruction, Action<Opcode>> {
                {Instruction.CODE_START, CodeStart},
                {Instruction.CODE_STOP, CodeStop},

                {Instruction.BINARY_ADD, BinaryAdd},
                {Instruction.BINARY_SUB, BinarySub},
                {Instruction.BINARY_MUL, BinaryMul},
                {Instruction.BINARY_DIV, BinaryDiv},
                {Instruction.BINARY_MOD, BinaryMod},

                {Instruction.BINARY_EQL, BinaryEqual},
                {Instruction.BINARY_NEQ, BinaryNotEqual},
                {Instruction.BINARY_GT, BinaryGreaterThan},
                {Instruction.BINARY_GTE, BinaryGreaterThanOrEqual},
                {Instruction.BINARY_LT, BinaryLessThan},
                {Instruction.BINARY_LTE, BinaryLessThanOrEqual},
                {Instruction.BINARY_OR, BinaryOr},
                {Instruction.BINARY_AND, BinaryAnd},

                {Instruction.BINARY_INDEX, BinaryIndex},

                {Instruction.UNARAY_NEG, UnaryNegative},
                {Instruction.UNARY_NOT, UnaryNot},

                {Instruction.RETURN, Return},
                {Instruction.CALL_FUNCTION, CallFunction},
                {Instruction.JUMP, Jump},
                {Instruction.JUMP_TRUE, JumpIfTrue},
                {Instruction.JUMP_FALSE, JumpIfFalse},
                {Instruction.JUMP_ABSOLUTE, JumpAbsolute},

                {Instruction.LOAD_CONST, LoadConst},
                {Instruction.LOAD_NAME, LoadName},
                {Instruction.LOAD_ATTR, LoadAttr},

                {Instruction.STORE_NAME, StoreName},
                {Instruction.STORE_NEW_NAME, StoreNewName},
                {Instruction.STORE_ATTR, StoreAttr},

                {Instruction.MAKE_ARR, MakeArray},
                {Instruction.MAKE_RECORD, MakeRecord},

                {Instruction.SYSCALL, Syscall}
            };
        }

        private Opcode GetNextOpcode() {
            return InstructionStream[instructionPointer++];
        }

        private Opcode GetCurrentOpcode() {
            return InstructionStream[instructionPointer];
        }

        private IValue Pop() {
            return Stack.Pop();
        }

        private void Push(IValue value) {
            Stack.Push(value);
        }

        public VirtualMachine(List<Opcode> instructionStream, Dictionary<int, IValue> constants) {
            this.InstructionStream = instructionStream;
            this.Constants = constants;

            initialiseHandlers();
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

        private void CodeStop(Opcode opcode) {
            executionBegun = false;
        }
    }
}
