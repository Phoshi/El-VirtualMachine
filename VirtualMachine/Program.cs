using System;
using System.IO;
using System.Linq;
using Speedycloud.Bytecode;

namespace Speedycloud.Runtime {
    class Program {
        static void Main(string[] args) {
            var bytecodeData = File.ReadAllText(args[0]);
            Console.WriteLine("Data:");
            Console.WriteLine(bytecodeData);
            var deserialiser = new BytecodeSerialiser();
            var data = deserialiser.Load(bytecodeData);

            var i = 0;
            var consts = data.Item2.ToDictionary(elem => i++);

            Console.WriteLine("Constants: ");
            foreach (var pair in consts) {
                Console.WriteLine("\t{0} = {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("Bytecode:");
            var bytecodeCount = 0;
            foreach (var opcode in data.Item1) {
                Console.WriteLine("\t" + bytecodeCount++ + " " + opcode);
            }

            var vm = new VirtualMachine(data.Item1.ToList(), consts);
            vm.Run();

            Console.ReadKey();
        }
    }
}
