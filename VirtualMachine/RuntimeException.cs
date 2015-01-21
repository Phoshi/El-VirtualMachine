using System;

namespace Speedycloud.Runtime {
    public class RuntimeException : Exception {
        public RuntimeException(string exceptionText) : base(exceptionText) {}
    }
}