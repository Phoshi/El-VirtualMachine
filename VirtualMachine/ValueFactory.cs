using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speedycloud.VirtualMachine.ValueTypes;

namespace Speedycloud.VirtualMachine {
    class ValueFactory {
        public IValue Make(long integer) {
            return new IntValue(integer);
        }

        public IValue Make(double number) {
            return new DoubleValue(number);
        }
    }
}
