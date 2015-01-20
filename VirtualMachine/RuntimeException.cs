using System;

namespace Speedycloud.VirtualMachine {
    public class RuntimeException : Exception {
        public RuntimeException(string exceptionText) : base(exceptionText) {}
    }
}