using StandardProject.Domain.Repositories;
using StandardProject.Domain.Repositories.User;
using StandardProject.Domain.Services.LoggedUser;
using StandardProject.Domain.Services.ServiceBus;

namespace StandardProject.Application.UseCases.User.Delete.Request;
public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
    private readonly IDeleteUserQueue _queue;
    private readonly IUserUpdateOnlyRepository _userUpdateRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public RequestDeleteUserUseCase(
        IDeleteUserQueue queue,
        IUserUpdateOnlyRepository userUpdateRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _queue = queue;
        _loggedUser = loggedUser;
        _userUpdateRepository = userUpdateRepository;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();

        var user = await _userUpdateRepository.GetById(loggedUser.Id);

        user.Active = false;
        _userUpdateRepository.Update(user);

        await _unitOfWork.Commit();

        await _queue.SendMessage(loggedUser);
    }
}
