using StandardProject.Communication.Requests;

namespace StandardProject.Application.UseCases.User.Update;
public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
