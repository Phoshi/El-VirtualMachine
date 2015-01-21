using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedycloud.Runtime;
using Speedycloud.Runtime.ValueTypes;
using ValueType = Speedycloud.Runtime.ValueType;

namespace Speedycloud.Language.Runtime.ValueTypes {
    class BooleanValue : IValue {
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
