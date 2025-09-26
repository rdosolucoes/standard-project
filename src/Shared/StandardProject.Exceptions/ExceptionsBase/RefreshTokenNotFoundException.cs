using System.Net;

namespace StandardProject.Exceptions.ExceptionsBase;
public class RefreshTokenNotFoundException : StandardProjectException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.EXPIRED_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
