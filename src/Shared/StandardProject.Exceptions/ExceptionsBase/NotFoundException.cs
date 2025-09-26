using System.Net;

namespace StandardProject.Exceptions.ExceptionsBase;
public class NotFoundException : StandardProjectException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}