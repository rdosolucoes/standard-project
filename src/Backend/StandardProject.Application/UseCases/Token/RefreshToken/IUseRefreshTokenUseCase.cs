using StandardProject.Communication.Requests;
using StandardProject.Communication.Responses;

namespace StandardProject.Application.UseCases.Token.RefreshToken;
public interface IUseRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}
