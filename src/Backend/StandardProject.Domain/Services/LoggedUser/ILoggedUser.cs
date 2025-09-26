using StandardProject.Domain.Entities;

namespace StandardProject.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    public Task<User> User();
}
