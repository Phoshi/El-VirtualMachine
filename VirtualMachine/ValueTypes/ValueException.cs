using System;

namespace Speedycloud.VirtualMachine.ValueTypes {
    internal class ValueException : Exception {
        public ValueException(ValueType expected, ValueType actual) : 
            base(string.Format("Incorrect value type found! Expected {0} but got {1}", expected, actual)) {}
    }
}