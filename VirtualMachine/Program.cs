using System;
using System.IO;
using System.Linq;
using System.Text;
using Speedycloud.Bytecode;

namespace Speedycloud.Runtime {
    class Program {
        private readonly static string LOG_PATH = "VM-" + DateTime.Now.ToString("yy-MM-dd-H-mm-ss") + ".log";
        private static int level = 0;

        private static StringBuilder log = new StringBuilder();

        public static void Log(string details) {
            var tabs = new string('\t', level);
            var logLine = tabs + "[{0}] {1}\n";
            var time = DateTime.Now.ToString("yy-MM-dd H:mm:ss.fffffff");
            //File.AppendAllText(LOG_PATH, string.Format(logLine, time, details));
            log.Append(string.Format(logLine, time, details));
        }

        public static void LogIn(string details) {
            Log(details);
            level++;
        }

        public static void LogOut(string details) {
            if (level > 0) {
                level--;
            }
            Log(details);
        }
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
            File.WriteAllText(LOG_PATH, log.ToString());
        }
    }
}
