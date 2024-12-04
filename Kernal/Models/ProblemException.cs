namespace SharedKernel;
public class ProblemException(Exception exception, string code) : Exception()
{
    public string? Code { get; } = code;
    public Exception Exception { get; } = exception;
}