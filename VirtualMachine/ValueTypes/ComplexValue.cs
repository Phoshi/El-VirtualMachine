using System.Collections.Generic;

namespace Speedycloud.Runtime.ValueTypes {
    public class ComplexValue : IValue {
        public IReadOnlyList<IValue> Slots { get { return new List<IValue>(slots).AsReadOnly();} }
        private readonly IValue[] slots;

        public ComplexValue(IValue[] slots) {
            this.slots = slots;
        }

        public ComplexValue Update(int slot, IValue value) {
            var newSlots = new List<IValue>(slots);
            newSlots[slot] = value;
            return new ComplexValue(newSlots.ToArray());
        }


        public ValueType Type { get { return ValueType.Complex; } }
        public long Integer { get { throw new ValueException(ValueType.Complex, ValueType.Integer); } }
        public double Double { get { throw new ValueException(ValueType.Complex, ValueType.Double); } }
        public bool Boolean { get { throw new ValueException(ValueType.Complex, ValueType.Boolean); } }
        public string String { get { throw new ValueException(ValueType.Complex, ValueType.String); } }
        public ArrayValue Array { get { throw new ValueException(ValueType.Complex, ValueType.Array); } }
        public ComplexValue Complex { get { return this; } }
    }
}