namespace StandardProject.Domain.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
