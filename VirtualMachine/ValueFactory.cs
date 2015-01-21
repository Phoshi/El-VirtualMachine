using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedycloud.Runtime.ValueTypes;
using Speedycloud.Language.Runtime.ValueTypes;

namespace Speedycloud.Runtime {
    class ValueFactory {
        public IValue Make(long integer) {
            return new IntValue(integer);
        }

        public IValue Make(double number) {
            return new DoubleValue(number);
        }

        public IValue Make(bool flag) {
            return new BooleanValue(flag);
        }
    }
}
