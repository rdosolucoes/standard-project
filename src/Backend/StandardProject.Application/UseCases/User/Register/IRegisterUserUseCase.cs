using StandardProject.Communication.Requests;
using StandardProject.Communication.Responses;

namespace StandardProject.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
