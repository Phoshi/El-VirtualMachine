﻿using System.Collections.Generic;
using System.Linq;
using Speedycloud.Bytecode.ValueTypes;
using Speedycloud.Runtime.ValueTypes;

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

        public IValue Make(string str) {
            return new StringValue(str);
        }

        public IValue Make(IEnumerable<IValue> arr) {
            return new ArrayValue(arr.ToArray());
        }
    }
}
