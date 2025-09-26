using System.Net;

namespace StandardProject.Exceptions.ExceptionsBase;
public class UnauthorizedException : StandardProjectException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}