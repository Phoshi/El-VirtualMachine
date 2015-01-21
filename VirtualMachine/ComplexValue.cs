using System.Collections.Generic;
using System.Linq;

namespace Speedycloud.Runtime {
    public class ComplexValue {
        public IReadOnlyList<IValue> Slots { get { return new List<IValue>(slots).AsReadOnly();} }
        private IValue[] slots;

        public ComplexValue(IValue[] slots) {
            this.slots = slots;
        }

        public ComplexValue Update(int slot, IValue value) {
            var newSlots = new List<IValue>(slots);
            newSlots[slot] = value;
            return new ComplexValue(slots.ToArray());
        }
    }
}