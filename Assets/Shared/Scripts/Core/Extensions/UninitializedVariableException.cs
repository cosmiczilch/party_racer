
public class UninitializedVariableException : System.Exception {
    public UninitializedVariableException() : base() { }
    public UninitializedVariableException(string message) : base(message) { }
    public UninitializedVariableException(string message, System.Exception innerException) : base(message, innerException) { }
}
