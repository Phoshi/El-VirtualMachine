using System;
using System.IO;
using System.Linq;
using Speedycloud.Bytecode;

namespace Speedycloud.Runtime {
    class Program {
        static void Main(string[] args) {
            var switches = args.Where(arg => arg[0] == '-').ToList();
            var filename = args.First(arg => arg[0] != '-');
            var bytecodeData = File.ReadAllText(filename);
            var deserialiser = new BytecodeSerialiser();
            var data = deserialiser.Load(bytecodeData);

            var i = 0;
            var consts = data.Item2.ToDictionary(elem => i++);

            if (switches.Contains("-v")) {
                Console.WriteLine("Constants: ");
                foreach (var pair in consts) {
                    Console.WriteLine("\t{0} = {1}", pair.Key, pair.Value);
                }

                Console.WriteLine("Bytecode:");
                var bytecodeCount = 0;
                foreach (var opcode in data.Item1) {
                    Console.WriteLine("\t" + bytecodeCount++ + " " + opcode);
                }
                Console.WriteLine("Program output:");
            }

            var vm = new VirtualMachine(data.Item1.ToList(), consts);
            vm.Run();

            if (switches.Contains("-v")) {
                Console.WriteLine();
                Console.WriteLine("Heap:");
                foreach (var heap in vm.Heap) {
                    Console.WriteLine(heap);
                }

                Console.ReadKey();
            }
        }
    }
}
