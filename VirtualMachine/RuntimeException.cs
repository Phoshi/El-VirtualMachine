using System;

namespace VirtualMachine {
    internal class RuntimeException : Exception {
        public RuntimeException(string exceptionText) : base(exceptionText) {}
    }
}