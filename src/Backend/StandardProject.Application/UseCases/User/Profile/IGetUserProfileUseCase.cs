using StandardProject.Communication.Responses;

namespace StandardProject.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
