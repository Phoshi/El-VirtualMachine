using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Speedycloud.VirtualMachine;
using Speedycloud.VirtualMachine.ValueTypes;

namespace VirtualMachineTests {
    [TestClass]
    public class VirtualMachineTests {
        private Dictionary<int, IValue> consts = new Dictionary<int, IValue> {
            {0, new IntValue(2)}
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
    }
}
