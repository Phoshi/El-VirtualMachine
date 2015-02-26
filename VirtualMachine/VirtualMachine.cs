using System;
using System.Collections.Generic;
using Speedycloud.Bytecode;
using Speedycloud.Bytecode.ValueTypes;
using Speedycloud.Runtime.Opcodes;
using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime {
    public class VirtualMachine {
        internal readonly Dictionary<int, IValue> Constants;

        internal ValueFactory ValueFactory = new ValueFactory();

        public Dictionary<int, IValue> Heap = new Dictionary<int, IValue>();

        public Stack<IValue> Stack = new Stack<IValue>();
        internal int CurrentStackFrame = 0;

        public NameTable CurrentNameTable = new NameTable();

        private readonly List<Opcode> instructionStream;
        internal int InstructionPointer = 0;

        internal bool ExecutionBegun = false;

        private readonly Dictionary<Instruction, IOpcodeHandler> handlers = new Dictionary<Instruction, IOpcodeHandler> {
                {Instruction.CODE_START, new CodeStart()},
                {Instruction.CODE_STOP, new CodeStop()},

                {Instruction.POP_TOP, new PopTop()},

                {Instruction.BINARY_ADD, new BinaryAdd()},
                {Instruction.BINARY_SUB, new BinarySub()},
                {Instruction.BINARY_MUL, new BinaryMul()},
                {Instruction.BINARY_DIV, new BinaryDiv()},
                {Instruction.BINARY_MOD, new BinaryMod()},

                {Instruction.BINARY_EQL, new BinaryEqual()},
                {Instruction.BINARY_NEQ, new BinaryNotEqual()},
                {Instruction.BINARY_GT, new BinaryGreaterThan()},
                {Instruction.BINARY_GTE, new BinaryGreaterThanOrEqual()},
                {Instruction.BINARY_LT, new BinaryLessThan()},
                {Instruction.BINARY_LTE, new BinaryLessThanOrEqual()},
                {Instruction.BINARY_OR, new BinaryOr()},
                {Instruction.BINARY_AND, new BinaryAnd()},

                {Instruction.BINARY_INDEX, new BinaryIndex()},
                {Instruction.BINARY_INDEX_UPDATE, new BinaryIndexUpdate()},

                {Instruction.UNARY_NEG, new UnaryNegative()},
                {Instruction.UNARY_NOT, new UnaryNot()},

                {Instruction.RETURN, new Return()},
                {Instruction.CALL_FUNCTION, new CallFunction()},
                {Instruction.JUMP, new Jump()},
                {Instruction.JUMP_TRUE, new JumpIf(true)},
                {Instruction.JUMP_FALSE, new JumpIf(false)},
                {Instruction.JUMP_ABSOLUTE, new Jump(absolute: true)},

                {Instruction.LOAD_CONST, new LoadConst()},
                {Instruction.LOAD_NAME, new LoadName()},
                {Instruction.LOAD_ATTR, new LoadAttribute()},

                {Instruction.STORE_NAME, new StoreName()},
                {Instruction.STORE_NEW_NAME, new StoreNewName()},
                {Instruction.STORE_ATTR, new StoreAttribute()},

                {Instruction.MAKE_ARR, new MakeArray()},
                {Instruction.MAKE_RECORD, new MakeRecord()},

                {Instruction.SYSCALL, new Syscall()}
            };

        private Opcode GetNextOpcode() {
            return instructionStream[InstructionPointer++];
        }

        private Opcode GetCurrentOpcode() {
            return instructionStream[InstructionPointer];
        }

        internal IValue Pop() {
            return Stack.Pop();
        }

        internal void Push(IValue value) {
            Stack.Push(value);
        }

        public VirtualMachine(List<Opcode> instructionStream, Dictionary<int, IValue> constants) {
            this.instructionStream = instructionStream;
            this.Constants = constants;
        }

        public void Run() {
            while (true) {
                Step();
                DoGarbageCollection();
                if (ExecutionBegun && GetCurrentOpcode().Instruction == Instruction.CODE_STOP) {
                    return;
                }
            }
        }

        public void Step() {
            var opcode = GetNextOpcode();
            if (!ExecutionBegun && opcode.Instruction != Instruction.CODE_START) {
                return;
            }

            handlers[opcode.Instruction].Accept(opcode, this);
        }

        private int nameIdentifier;
        private Random rng = new Random();
        internal int GetNewNameIdentifier() {
            return rng.Next();
        }

        private void DoGarbageCollection() {
            foreach (var allocation in new Dictionary<int, IValue>(Heap)) {
                if (!CurrentNameTable.ContainsReferenceTo(allocation.Key)) {
                    Heap.Remove(allocation.Key);
                }
            }
        }

    }
}
