using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedycloud.Runtime;
using Speedycloud.Runtime.ValueTypes;
using ValueType = Speedycloud.Runtime.ValueType;

namespace Speedycloud.Language.Runtime.ValueTypes {
    public class StringValue : IValue {
        public ValueType Type { get {return ValueType.String;} }
        public long Integer { get { throw new ValueException(ValueType.String, ValueType.Integer); } }
        public double Double { get { throw new ValueException(ValueType.String, ValueType.Double); } }
        public bool Boolean { get { throw new ValueException(ValueType.String, ValueType.Boolean); } }
        public string String { get; private set; }
        public ArrayValue Array { get { throw new ValueException(ValueType.String, ValueType.Array); } }
        public ComplexValue Complex { get { throw new ValueException(ValueType.String, ValueType.Complex); } }

        public StringValue(string str) {
            String = str;
        }
    }
}
