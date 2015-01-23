namespace Speedycloud.Runtime.ValueTypes {
    public class BooleanValue : IValue {
        public ValueType Type { get { return ValueType.Boolean; } }
        public long Integer {
            get { throw new ValueException(ValueType.Boolean, ValueType.Integer); }
        }
        public double Double { get { throw new ValueException(ValueType.Boolean, ValueType.Double); } }
        public bool Boolean { get; private set; }
        public string String { get { throw new ValueException(ValueType.Boolean, ValueType.String); } }
        public ArrayValue Array { get { throw new ValueException(ValueType.Boolean, ValueType.Array); } }
        public ComplexValue Complex { get { throw new ValueException(ValueType.Boolean, ValueType.Complex); } }

        public BooleanValue(bool value) {
            Boolean = value;
        }
    }
}
