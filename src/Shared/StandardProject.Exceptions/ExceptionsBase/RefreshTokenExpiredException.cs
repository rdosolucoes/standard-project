using System.Net;

namespace StandardProject.Exceptions.ExceptionsBase;
public class RefreshTokenExpiredException : StandardProjectException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
