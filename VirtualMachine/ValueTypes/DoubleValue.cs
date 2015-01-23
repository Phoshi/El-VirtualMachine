namespace Speedycloud.Runtime.ValueTypes {
    public class DoubleValue : IValue {
        public ValueType Type {
            get { return ValueType.Double; }
        }

        public long Integer { get {throw new ValueException(ValueType.Double, ValueType.Integer);} }

        public double Double { get; private set; }

        public bool Boolean {
            get { throw new ValueException(ValueType.Double, ValueType.Boolean); }
        }

        public string String {
            get { throw new ValueException(ValueType.Double, ValueType.String); }
        }

        public ArrayValue Array {
            get { throw new ValueException(ValueType.Double, ValueType.Array); }
        }

        public ComplexValue Complex {
            get { throw new ValueException(ValueType.Double, ValueType.Complex); }
        }

        public DoubleValue(double value) {
            Double = value;
        }
    }
}
