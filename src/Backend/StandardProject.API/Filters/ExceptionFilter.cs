using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StandardProject.Communication.Responses;
using StandardProject.Exceptions;
using StandardProject.Exceptions.ExceptionsBase;

namespace StandardProject.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is StandardProjectException standardProjectException)
            HandleProjectException(standardProjectException, context);
        else
            ThrowUnknowException(context);
    }

    private static void HandleProjectException(StandardProjectException standardProjectException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)standardProjectException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(standardProjectException.GetErrorMessages()));
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }
}
