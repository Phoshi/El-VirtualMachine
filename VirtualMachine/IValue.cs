namespace Speedycloud.VirtualMachine {
    public interface IValue {
        ValueType Type { get; }
        long Integer { get; }
        double Double { get; }
        bool Boolean { get; }
        string String { get; }
        ArrayValue Array { get; }
        ComplexValue Complex { get; }
    }

    public enum ValueType {
        Integer,
        Double,
        Boolean,
        String,
        Array,
        Complex,
    }
}