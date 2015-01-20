namespace VirtualMachine {
    internal interface IValue {
        long Integer { get; }
        double Double { get; }
        bool Boolean { get; }
        string String { get; }
        ArrayValue Array { get; }
        ComplexValue Complex { get; }
    }
}