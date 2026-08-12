
using Test.Common.Domain;

namespace Test.Common.Application.ExceptionHandeling;

public class TestExpection : Exception
{

    public TestExpection(string message, Error error, Exception? innerException = default) : base("Application Exception", innerException)
    {
        Error = error;
        Messages = message;
    }
    public string Messages { get; }
    public Error Error { get; }
}
