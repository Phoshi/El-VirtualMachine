namespace Speedycloud.Runtime {
    public enum Instruction {
        CODE_START,
        CODE_STOP,

        BINARY_ADD,
        BINARY_SUB,
        BINARY_MUL,
        BINARY_DIV,
        BINARY_MOD,

        BINARY_EQL,
        BINARY_NEQ,
        BINARY_GT,
        BINARY_LT,
        BINARY_GTE,
        BINARY_LTE,
        BINARY_AND,
        BINARY_OR,

        BINARY_INDEX,

        UNARAY_NEG,
        UNARY_NOT,

        POP_TOP,

        RETURN,
        CALL_FUNCTION,
        JUMP,
        JUMP_TRUE,
        JUMP_FALSE,
        JUMP_ABSOLUTE,

        LOAD_CONST,
        LOAD_NAME,
        LOAD_ATTR,

        STORE_NAME,
        STORE_NEW_NAME,
        STORE_ATTR,

        MAKE_ARR,
        MAKE_RECORD,

        SYSCALL,
    }
}