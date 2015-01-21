using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
