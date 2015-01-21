using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Speedycloud.Runtime {
    public class Opcode {
        public static Opcode Of(Instruction instruction) {
            return new Opcode(instruction);
        }
        public Instruction Instruction { get; internal set; }
        public IReadOnlyList<int> OpArgs { get { return opargs.AsReadOnly(); } }
        private readonly List<int> opargs;

        public Opcode(Instruction instruction, params int[] args) {
            this.Instruction = instruction;
            this.opargs = new List<int>(args);
        }
    }
}
