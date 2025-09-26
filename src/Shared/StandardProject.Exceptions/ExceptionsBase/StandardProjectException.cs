using System.Net;

namespace StandardProject.Exceptions.ExceptionsBase;
public abstract class StandardProjectException : SystemException
{
    protected StandardProjectException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
