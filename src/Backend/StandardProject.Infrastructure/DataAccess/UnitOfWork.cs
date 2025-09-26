using StandardProject.Domain.Repositories;

namespace StandardProject.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly StandardProjectDbContext _dbContext;

    public UnitOfWork(StandardProjectDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
