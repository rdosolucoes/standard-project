using StandardProject.Communication.Requests;
using StandardProject.Communication.Responses;

namespace StandardProject.Application.UseCases.Login.DoLogin;
public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
