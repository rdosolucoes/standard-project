using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using StandardProject.Application.UseCases.Login.DoLogin;
using StandardProject.Application.UseCases.Login.External;
using StandardProject.Communication.Requests;
using StandardProject.Communication.Responses;
using System.Security.Claims;

namespace StandardProject.API.Controllers;

public class LoginController : StandardProjectBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpGet]
    [Route("google")]
    public async Task<IActionResult> LoginGoogle(
        string returnUrl,
        [FromServices] IExternalLoginUseCase useCase)
    {
        var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if(IsNotAuthenticated(authenticate))
        {
            return Challenge(GoogleDefaults.AuthenticationScheme);
        }
        else
        {
            var claims = authenticate.Principal!.Identities.First().Claims;

            var name = claims.First(c => c.Type == ClaimTypes.Name).Value;
            var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

            var token = await useCase.Execute(name, email);

            return Redirect($"{returnUrl}/{token}");
        }
    }
}
