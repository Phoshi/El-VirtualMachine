using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speedycloud.Runtime.ValueTypes {
    public class IntValue : IValue {
        public ValueType Type {
            get { return ValueType.Integer; }
        }

        public long Integer { get; private set; }

        public double Double {
            get { return Integer; }
        }

        public bool Boolean {
            get { throw new ValueException(ValueType.Integer, ValueType.Boolean); }
        }

        public string String {
            get { throw new ValueException(ValueType.Integer, ValueType.String); }
        }

        public ArrayValue Array {
            get { throw new ValueException(ValueType.Integer, ValueType.Array); }
        }

        public ComplexValue Complex {
            get { throw new ValueException(ValueType.Integer, ValueType.Complex); }
        }

        public IntValue(long value) {
            Integer = value;
        }

        protected bool Equals(IntValue other) {
            return Integer == other.Integer;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntValue) obj);
        }

        public override int GetHashCode() {
            return Integer.GetHashCode();
        }
    }
}
