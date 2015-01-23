using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Speedycloud.Language.Runtime.ValueTypes;
using Speedycloud.Runtime;
using Speedycloud.Runtime.ValueTypes;

namespace VirtualMachineTests {
    [TestClass]
    public class VirtualMachineTests {
        private Dictionary<int, IValue> consts = new Dictionary<int, IValue> {
            {0, new IntValue(2)},
            {1, new IntValue(1)},
            {2, new DoubleValue(1.5)},
            {3, new DoubleValue(3.0)},
            {4, new BooleanValue(true)},
            {5, new BooleanValue(false)},
            {6, new ArrayValue(new IValue[]{new IntValue(0), new IntValue(3), new IntValue(4)})},

            {7, new StringValue("Hello, world!")},
            {8, new IntValue(0)},

            {9, new IntValue('\n')},
            {10, new StringValue("")},

            //Function references
            {101, new IntValue(0)},

            //Variable names
            {201, new StringValue("x")},
            {202, new StringValue("word")},
        };

        private VirtualMachine VM(params Opcode[] codes) {
            var vm = new VirtualMachine(
                new[]{Opcode.Of(Instruction.CODE_START)}.Concat(codes).Concat(new[]{Opcode.Of(Instruction.CODE_STOP)}).ToList(),
                consts);
            vm.Run();
            return vm;
        }
            
        [TestMethod]
        public void StartsUp() {
            var vm = new VirtualMachine(new List<Opcode>(), consts);
        }

        [TestMethod]
        public void CodeStartAndStop() {
            var vm = new VirtualMachine(new List<Opcode>{new Opcode(Instruction.CODE_START), new Opcode(Instruction.CODE_STOP)}, consts);
            vm.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeException))]
        public void CodeStartTwice() {
            var vm = VM(Opcode.Of(Instruction.CODE_START));
        }

        [TestMethod]
        public void LoadConstant() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0));

            Assert.AreEqual(2, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void Addition() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_ADD));

            Assert.AreEqual(4, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void AdditionDouble() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_ADD));
            Assert.AreEqual(3, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void AdditionMixedType() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_ADD));
            Assert.AreEqual(3.5, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void Subtraction() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_SUB));

            Assert.AreEqual(1, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void SubtractionDouble() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_SUB));

            Assert.AreEqual(0, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void SubtractionMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_SUB));

            Assert.AreEqual(-0.5, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void Multiplication() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_MUL));

            Assert.AreEqual(4, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void MultiplicationDouble() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_MUL));

            Assert.AreEqual(2.25, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void MultiplicationMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_MUL));

            Assert.AreEqual(1.5, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void Division() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_DIV));

            Assert.AreEqual(1, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void DivisionDouble() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_DIV));

            Assert.AreEqual(1, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void DivisionMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_DIV));

            Assert.AreEqual(2/3d, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void Modulus() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_MOD));
            var vm2 = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_MOD));

            Assert.AreEqual(0, vm.Stack.Peek().Integer);
            Assert.AreEqual(1, vm2.Stack.Peek().Integer);
        }

        [TestMethod]
        public void ModulusDouble() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_MOD));

            Assert.AreEqual(1.5, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void ModulusMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_MOD));

            Assert.AreEqual(0.5, vm.Stack.Peek().Double);
        }

        [TestMethod]
        public void EqualIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_EQL));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void EqualNotIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_EQL));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void EqualDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_EQL));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void EqualNotDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_EQL));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void EqualNotTypes() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_EQL));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void NotEqualIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_NEQ));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void NotEqualNotIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_NEQ));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void NotEqualDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_NEQ));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void NotEqualNotDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_NEQ));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void NotEqualNotTypes() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_NEQ));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_GTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 2), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_LT));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanOrEqualIntegers() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 1), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanOrEqualDoubles() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 2),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void LessThanOrEqualMixed() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 3),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 3), new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_LTE));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void Or() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.LOAD_CONST, 5),
                Opcode.Of(Instruction.BINARY_OR));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.LOAD_CONST, 5),
                Opcode.Of(Instruction.BINARY_OR));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.LOAD_CONST, 4),
                Opcode.Of(Instruction.BINARY_OR));

            Assert.IsTrue(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.LOAD_CONST, 4),
                Opcode.Of(Instruction.BINARY_OR));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void And() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.LOAD_CONST, 5),
                Opcode.Of(Instruction.BINARY_AND));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.LOAD_CONST, 5),
                Opcode.Of(Instruction.BINARY_AND));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.LOAD_CONST, 4),
                Opcode.Of(Instruction.BINARY_AND));

            Assert.IsFalse(vm.Stack.Peek().Boolean);

            vm = VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.LOAD_CONST, 4),
                Opcode.Of(Instruction.BINARY_AND));

            Assert.IsTrue(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void BinaryIndex() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 6), new Opcode(Instruction.LOAD_CONST, 1),
                Opcode.Of(Instruction.BINARY_INDEX));
            Assert.AreEqual(3, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void UnaryNegative() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), Opcode.Of(Instruction.UNARY_NEG));

            Assert.AreEqual(-2, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void UnaryNot() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 4), Opcode.Of(Instruction.UNARY_NOT));

            Assert.IsFalse(vm.Stack.Peek().Boolean);
        }

        [TestMethod]
        public void CallFunction() {
            var vm = new VirtualMachine(new List<Opcode> {
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_ADD),
                new Opcode(Instruction.CODE_STOP),
                new Opcode(Instruction.CODE_START),
                new Opcode(Instruction.CALL_FUNCTION, 101, 0)
            }, consts);

            vm.Run();

            Assert.AreEqual(4, vm.Stack.Pop().Integer);
            Assert.AreEqual(6, vm.Stack.Pop().Integer);
            Assert.AreEqual(0, vm.Stack.Pop().Integer);
        }

        [TestMethod]
        public void ReturnFromFunction() {
            var vm = new VirtualMachine(new List<Opcode> {
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_ADD),
                Opcode.Of(Instruction.RETURN),
                new Opcode(Instruction.CODE_START),
                new Opcode(Instruction.CALL_FUNCTION, 101, 0),
                new Opcode(Instruction.CODE_STOP),
            }, consts);

            vm.Run();

            Assert.AreEqual(4, vm.Stack.Pop().Integer);
            Assert.AreEqual(0, vm.Stack.Count);
        }

        [TestMethod]
        public void Jump() {
            VM(new Opcode(Instruction.JUMP, 1), Opcode.Of(Instruction.CODE_START));
        }

        [TestMethod]
        public void JumpIfTrue() {
            VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.JUMP_TRUE, 1),
                Opcode.Of(Instruction.CODE_START));


            VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.JUMP_TRUE, 1),
                Opcode.Of(Instruction.CODE_STOP),
                Opcode.Of(Instruction.CODE_START));
        }

        [TestMethod]
        public void JumpIfFalse() {
            VM(new Opcode(Instruction.LOAD_CONST, 5), new Opcode(Instruction.JUMP_FALSE, 1),
                Opcode.Of(Instruction.CODE_START));


            VM(new Opcode(Instruction.LOAD_CONST, 4), new Opcode(Instruction.JUMP_FALSE, 1),
                Opcode.Of(Instruction.CODE_STOP),
                Opcode.Of(Instruction.CODE_START));
        }

        [TestMethod]
        public void JumpAbsolute() {
            var vm = VM(new Opcode(Instruction.JUMP_ABSOLUTE, 5),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 0),
                Opcode.Of(Instruction.BINARY_ADD));

            Assert.AreEqual(0, vm.Stack.Count);
        }

        [TestMethod]
        public void StoreNewName() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.STORE_NEW_NAME, 1, 201));

            Assert.IsTrue(vm.CurrentNameTable.Lookup(1).String == "x");
            Assert.AreEqual(2, vm.Heap[vm.CurrentNameTable.Lookup(1).Value].Integer);
        }

        [TestMethod]
        public void StoreName() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), new Opcode(Instruction.STORE_NEW_NAME, 1, 201), 
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.STORE_NAME, 1));

            Assert.IsTrue(vm.CurrentNameTable.Lookup(1).String == "x");
            Assert.AreEqual(1, vm.Heap[vm.CurrentNameTable.Lookup(1).Value].Integer);
        }

        [TestMethod]
        public void LoadName() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 0), 
                new Opcode(Instruction.STORE_NEW_NAME, 1, 201),
                new Opcode(Instruction.LOAD_NAME, 1),
                new Opcode(Instruction.LOAD_NAME, 1),
                new Opcode(Instruction.BINARY_ADD));

            Assert.AreEqual(4, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void MakeRecord() {
            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.LOAD_CONST, 2),
                new Opcode(Instruction.MAKE_RECORD, 3));

            Assert.AreEqual(2, vm.Stack.Peek().Complex.Slots[0].Integer);
            Assert.AreEqual(1, vm.Stack.Peek().Complex.Slots[1].Integer);
            Assert.AreEqual(1.5, vm.Stack.Peek().Complex.Slots[2].Double);
        }

        [TestMethod]
        public void LoadAttr() {
            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.LOAD_CONST, 2),
                new Opcode(Instruction.MAKE_RECORD, 3),
                new Opcode(Instruction.LOAD_ATTR, 1));

            Assert.AreEqual(1, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void StoreAttr() {
            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.LOAD_CONST, 2),
                new Opcode(Instruction.MAKE_RECORD, 3),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.STORE_ATTR, 1));

            Assert.AreEqual(2, vm.Stack.Peek().Complex.Slots[1].Integer);
        }

        [TestMethod]
        public void MakeArray() {
            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.LOAD_CONST, 1),
                new Opcode(Instruction.MAKE_ARR, 3));

            Assert.AreEqual(2, vm.Stack.Peek().Array.Contents[0].Integer);
            Assert.AreEqual(1, vm.Stack.Peek().Array.Contents[1].Integer);
            Assert.AreEqual(1, vm.Stack.Peek().Array.Contents[2].Integer);
            Assert.AreEqual(3, vm.Stack.Peek().Array.Contents.Count);
        }

        [TestMethod]
        public void GetArrayLength() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 6),
                new Opcode(Instruction.LOAD_ATTR, 0));

            Assert.AreEqual(3, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void GetStringLength() {
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 7),
                new Opcode(Instruction.LOAD_ATTR, 0));

            Assert.AreEqual(13, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void PrintCharacter() {
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            var vm = VM(new Opcode(Instruction.LOAD_CONST, 7),
                new Opcode(Instruction.LOAD_CONST, 8),
                new Opcode(Instruction.BINARY_INDEX),
                new Opcode(Instruction.SYSCALL, 0));

            Assert.AreEqual("H", textWriter.ToString());
        }

        [TestMethod]
        public void PrintWord() {
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);
            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 8), //Set up counter
                new Opcode(Instruction.STORE_NEW_NAME, 0, 201), //Push it to a variable

                new Opcode(Instruction.LOAD_NAME, 0), //Load the current counter
                new Opcode(Instruction.LOAD_CONST, 7), //Load our string
                new Opcode(Instruction.LOAD_ATTR, 0), //Take the length
                new Opcode(Instruction.BINARY_EQL), //Check if they're the same
                new Opcode(Instruction.JUMP_FALSE, 1), //Skip the next instruction if they aren't
                new Opcode(Instruction.CODE_STOP),
                new Opcode(Instruction.LOAD_CONST, 7), //Load our string
                new Opcode(Instruction.LOAD_NAME, 0), //Load the current counter
                new Opcode(Instruction.BINARY_INDEX), //Index into the string
                new Opcode(Instruction.SYSCALL, 0), //And print it
                new Opcode(Instruction.LOAD_NAME, 0), //Load the current counter
                new Opcode(Instruction.LOAD_CONST, 1), //Load the increment value
                new Opcode(Instruction.BINARY_ADD), //Add them together
                new Opcode(Instruction.STORE_NAME, 0), //And store back in the variable
                new Opcode(Instruction.JUMP_ABSOLUTE, 3)
                ); 

            Assert.AreEqual("Hello, world!", textWriter.ToString());
        }

        [TestMethod]
        public void ReadWord() {
            var textReader = new StringReader("Hello, world\n");

            Console.SetIn(textReader);

            var vm = VM(
                new Opcode(Instruction.LOAD_CONST, 10), //Load empty string
                new Opcode(Instruction.STORE_NEW_NAME, 29, 202), //Store in variable word

                new Opcode(Instruction.LOAD_CONST, 8), //Load zero
                new Opcode(Instruction.STORE_NEW_NAME, 28, 201), //Store in character variable

                new Opcode(Instruction.SYSCALL, 1), //Read in char from stdin
                new Opcode(Instruction.STORE_NAME, 28), //Store in variable x

                new Opcode(Instruction.LOAD_NAME, 28), //Load character
                new Opcode(Instruction.LOAD_CONST, 9), //Load newline character
                new Opcode(Instruction.BINARY_EQL), //if they aren't equal
                new Opcode(Instruction.JUMP_FALSE, 1), //don't end
                new Opcode(Instruction.CODE_STOP),

                new Opcode(Instruction.LOAD_NAME, 29), //Load word
                new Opcode(Instruction.LOAD_NAME, 28), //Load character
                new Opcode(Instruction.BINARY_ADD), //Concatenate them
                new Opcode(Instruction.STORE_NAME, 29), //Put back in word
                new Opcode(Instruction.JUMP, -12)
                );

            Assert.AreEqual("Hello, world", vm.Heap[vm.CurrentNameTable.Lookup(29).Value].String);
        }

        [TestMethod]
        public void AppendToArray() {
            var vm = VM(
                new Opcode(Instruction.MAKE_ARR, 0),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.BINARY_ADD)
                );

            Assert.AreEqual(2, vm.Stack.Peek().Array.Contents[0].Integer);
        }

        [TestMethod]
        public void FunctionWithParameters() {
            var vm = new VirtualMachine(new List<Opcode> {
                new Opcode(Instruction.LOAD_NAME, 0),
                new Opcode(Instruction.LOAD_NAME, 1),
                new Opcode(Instruction.BINARY_ADD),
                new Opcode(Instruction.RETURN),
                new Opcode(Instruction.CODE_START),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.CALL_FUNCTION, 101, 2),
                new Opcode(Instruction.CODE_STOP)
            }, consts);
            vm.Run();

            Assert.AreEqual(4, vm.Stack.Peek().Integer);
        }

        [TestMethod]
        public void GarbageCollector() {
            var vm = new VirtualMachine(new List<Opcode> {
                new Opcode(Instruction.LOAD_NAME, 0),
                new Opcode(Instruction.STORE_NEW_NAME, 57, 201),
                new Opcode(Instruction.LOAD_NAME, 57),
                new Opcode(Instruction.LOAD_NAME, 1),
                new Opcode(Instruction.BINARY_ADD),
                new Opcode(Instruction.RETURN),
                new Opcode(Instruction.CODE_START),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.STORE_NEW_NAME, 57, 201),
                new Opcode(Instruction.LOAD_NAME, 57),
                new Opcode(Instruction.LOAD_CONST, 0),
                new Opcode(Instruction.CALL_FUNCTION, 101, 2),
                new Opcode(Instruction.CODE_STOP)
            }, consts);
            vm.Run();

            Assert.AreEqual(4, vm.Stack.Peek().Integer);
            Assert.AreEqual(1, vm.Heap.Count);
        }
    }
}
