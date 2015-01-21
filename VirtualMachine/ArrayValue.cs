using System.Collections.Generic;

namespace Speedycloud.Runtime {
    public class ArrayValue {
        public IReadOnlyList<IValue> Array { get { return new List<IValue>(arr).AsReadOnly();} }
        private IValue[] arr;

        public ArrayValue(IValue[] arr) {
            this.arr = arr;
        }

        public ArrayValue Update(int pos, IValue val) {
            var newArr = new List<IValue>(arr);
            newArr[pos] = val;
            return new ArrayValue(arr);
        }
    }
}