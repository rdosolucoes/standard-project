using Microsoft.AspNetCore.Mvc;
using StandardProject.Application.UseCases.Token.RefreshToken;
using StandardProject.Communication.Requests;
using StandardProject.Communication.Responses;

namespace StandardProject.API.Controllers;

public class TokenController : StandardProjectBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
