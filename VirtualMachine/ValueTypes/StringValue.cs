using System.Linq;

namespace Speedycloud.Runtime.ValueTypes {
    public class StringValue : IValue {
        public ValueType Type { get {return ValueType.String;} }
        public long Integer { get { throw new ValueException(ValueType.String, ValueType.Integer); } }
        public double Double { get { throw new ValueException(ValueType.String, ValueType.Double); } }
        public bool Boolean { get { throw new ValueException(ValueType.String, ValueType.Boolean); } }
        public string String { get; private set; }
        public ArrayValue Array { get { return new ArrayValue(String.Select<char, IValue>(c=>new IntValue(c)).ToArray());} }
        public ComplexValue Complex { get { return new ComplexValue(new IValue[]{new IntValue(String.Length)});} }

        public StringValue(string str) {
            String = str;
        }
    }
}
