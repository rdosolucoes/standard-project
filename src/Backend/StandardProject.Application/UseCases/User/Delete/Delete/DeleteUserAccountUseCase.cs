using StandardProject.Domain.Repositories;
using StandardProject.Domain.Repositories.User;
using StandardProject.Domain.Services.Storage;

namespace StandardProject.Application.UseCases.User.Delete.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IBlobStorageService _blobStorageService;

    public DeleteUserAccountUseCase(
        IUserDeleteOnlyRepository repository,
        //IBlobStorageService blobStorageService,
        IUnitOfWork unitOfWork)
    {
        //_blobStorageService = blobStorageService;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userIdentifier)
    {
        //await _blobStorageService.DeleteContainer(userIdentifier);

        await _repository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}
