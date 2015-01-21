using System;
using System.Collections.Generic;

namespace Speedycloud.Runtime {
    public class VirtualMachine {
        private readonly Dictionary<int, IValue> Constants;

        private ValueFactory valueFactory = new ValueFactory();

        public Dictionary<int, IValue> Heap = new Dictionary<int, IValue>();

        public Stack<IValue> Stack = new Stack<IValue>();
        private int currentStackFrame = 0;

        public NameTable CurrentNameTable = new NameTable();

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

                {Instruction.UNARY_NEG, UnaryNegative},
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

        private void Syscall(Opcode obj) {
            throw new NotImplementedException();
        }

        private void MakeRecord(Opcode obj) {
            throw new NotImplementedException();
        }

        private void MakeArray(Opcode obj) {
            throw new NotImplementedException();
        }

        private void StoreAttr(Opcode obj) {
            throw new NotImplementedException();
        }

        private void StoreNewName(Opcode obj) {
            var nameId = obj.OpArgs[0];
            var nameString = Constants[obj.OpArgs[1]];

            var name = new Name(nameString.String, GetNewNameIdentifier(), StorageType.Heap);

            CurrentNameTable.New(nameId, name);

            Heap[name.Value] = Pop();
        }

        private void StoreName(Opcode obj) {
            var nameId = obj.OpArgs[0];

            var name = CurrentNameTable.Lookup(nameId);
            Heap[name.Value] = Pop();
        }

        private void LoadAttr(Opcode obj) {
            throw new NotImplementedException();
        }

        private void LoadName(Opcode obj) {
            var nameId = obj.OpArgs[0];

            var name = CurrentNameTable.Lookup(nameId);
            Push(Heap[name.Value]);
        }

        private void LoadConst(Opcode obj) {
            var constant = Constants[obj.OpArgs[0]];

            Push(constant);
        }

        private void JumpAbsolute(Opcode obj) {
            instructionPointer = obj.OpArgs[0];
        }

        private void JumpIfFalse(Opcode obj) {
            var flag = Pop();
            if (!flag.Boolean) {
                instructionPointer += obj.OpArgs[0];
            }
        }

        private void JumpIfTrue(Opcode obj) {
            var flag = Pop();
            if (flag.Boolean) {
                instructionPointer += obj.OpArgs[0];
            }
        }

        private void Jump(Opcode obj) {
            instructionPointer += obj.OpArgs[0];
        }

        private void CallFunction(Opcode obj) {
            var target = Constants[obj.OpArgs[0]];
            Push(valueFactory.Make(currentStackFrame));
            Push(valueFactory.Make(instructionPointer));
            currentStackFrame = Stack.Count;
            instructionPointer = (int) target.Integer;
        }

        private void Return(Opcode obj) {
            var returnValue = Pop();
            while (Stack.Count > currentStackFrame) {
                Stack.Pop();
            }
            var newInstructionPointer = Pop();
            var oldStackFrame = Pop();

            currentStackFrame = (int) oldStackFrame.Integer;
            instructionPointer = (int) newInstructionPointer.Integer;
            Push(returnValue);
        }

        private void UnaryNot(Opcode obj) {
            var flag = Pop();
            Push(valueFactory.Make(!flag.Boolean));
        }

        private void UnaryNegative(Opcode obj) {
            var number = Pop();
            if (number.Type == ValueType.Integer) {
                Push(valueFactory.Make(-number.Integer));
            }
            else {
                Push(valueFactory.Make(-number.Double));
            }
        }

        private void BinaryIndex(Opcode obj) {
            var index = Pop();
            var array = Pop();
            Push(array.Array.Contents[(int) index.Integer]);
        }

        private void BinaryAnd(Opcode obj) {
            var val1 = Pop();
            var val2 = Pop();
            Push(valueFactory.Make(val1.Boolean && val2.Boolean));
        }

        private void BinaryOr(Opcode obj) {
            var val1 = Pop();
            var val2 = Pop();
            Push(valueFactory.Make(val1.Boolean || val2.Boolean));
        }

        private void BinaryLessThanOrEqual(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();

            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer <= val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer <= val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double <= val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double <= val2.Double));
                }
            }
        }

        private void BinaryLessThan(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();

            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer < val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer < val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double < val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double < val2.Double));
                }
            }
        }

        private void BinaryGreaterThanOrEqual(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();

            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer >= val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer >= val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double >= val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double >= val2.Double));
                }
            }
        }

        private void BinaryGreaterThan(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();

            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer > val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer > val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double > val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double > val2.Double));
                }
            }
        }

        private void BinaryNotEqual(Opcode obj) {
            BinaryEqual(obj);
            Push(valueFactory.Make(!Pop().Boolean));
        }

        private void BinaryEqual(Opcode obj) {
            var val1 = Pop();
            var val2 = Pop();
            if (val1.Type != val2.Type) {
                Push(valueFactory.Make(false));
                return;
            }

            Push(valueFactory.Make(val1.Equals(val2)));
        }

        private void BinaryMod(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();
            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer % val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer % val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double % val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double % val2.Double));
                }
            }
        }

        private void BinaryDiv(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();
            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer / val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer / val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double / val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double / val2.Double));
                }
            }
        }

        private void BinaryMul(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();
            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer * val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer * val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double * val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double * val2.Double));
                }
            }
        }

        private void BinarySub(Opcode obj) {
            var val2 = Pop();
            var val1 = Pop();
            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer - val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer - val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double - val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double - val2.Double));
                }
            }
        }

        private void BinaryAdd(Opcode opcode) {
            var val1 = Pop();
            var val2 = Pop();
            if (val1.Type == ValueType.Integer) {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Integer + val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Integer + val2.Double));
                }
            }
            else {
                if (val2.Type == ValueType.Integer) {
                    Push(valueFactory.Make(val1.Double + val2.Integer));
                }
                else {
                    Push(valueFactory.Make(val1.Double + val2.Double));
                }
            }
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
                if (executionBegun && GetCurrentOpcode().Instruction == Instruction.CODE_STOP) {
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

        private int nameIdentifier = 0;
        private int GetNewNameIdentifier() {
            return nameIdentifier++;
        }

    }
}
