using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StandardProject.Domain.Extensions;

namespace StandardProject.API.Controllers;

[Route("[controller]")]
[ApiController]
public class StandardProjectBaseController : ControllerBase
{
    protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
    {
        return !authenticate.Succeeded
            || authenticate.Principal is null
            || !authenticate.Principal.Identities.Any(id => id.IsAuthenticated);
    }
}
