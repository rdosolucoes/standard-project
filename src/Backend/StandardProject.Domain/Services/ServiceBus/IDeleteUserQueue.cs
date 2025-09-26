using StandardProject.Domain.Entities;

namespace StandardProject.Domain.Services.ServiceBus;
public interface IDeleteUserQueue
{
    Task SendMessage(User user);
}
