namespace Speedycloud.Runtime {
    public class Name {
        public string String { get; internal set; }
        public int Value { get; internal set; }
        public StorageType Type { get; internal set; }

        public Name(string str, int val, StorageType type) {
            String = str;
            Value = val;
            Type = type;
        }
    }

    public enum StorageType {
        Heap,
        Stack,
    }
}