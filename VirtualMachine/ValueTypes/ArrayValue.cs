﻿using System.Collections.Generic;
using Speedycloud.Runtime.ValueTypes;

namespace Speedycloud.Runtime {
    public class ArrayValue : IValue {
        public IReadOnlyList<IValue> Contents { get { return new List<IValue>(arr).AsReadOnly();} }
        private IValue[] arr;

        public ArrayValue(IValue[] arr) {
            this.arr = arr;
        }

        public ArrayValue Update(int pos, IValue val) {
            var newArr = new List<IValue>(arr);
            newArr[pos] = val;
            return new ArrayValue(arr);
        }

        public ValueType Type { get {return ValueType.Array;} }
        public long Integer { get { throw new ValueException(ValueType.Array, ValueType.Integer); } }
        public double Double { get { throw new ValueException(ValueType.Array, ValueType.Double); } }
        public bool Boolean { get { throw new ValueException(ValueType.Array, ValueType.Boolean); } }
        public string String { get { throw new ValueException(ValueType.Array, ValueType.String); } }
        public ArrayValue Array { get { return this; } }
        public ComplexValue Complex { get { throw new ValueException(ValueType.Array, ValueType.Complex); } }
    }
}