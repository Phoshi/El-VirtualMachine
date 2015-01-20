namespace Speedycloud.VirtualMachine {
    internal class Name {
        public string String { get; internal set; }
        public int Value { get; internal set; }
        public StorageType Type { get; internal set; }
    }

    internal enum StorageType {
        Heap,
        Stack,
    }
}