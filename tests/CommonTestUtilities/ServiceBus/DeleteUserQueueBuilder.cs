using Moq;
using StandardProject.Domain.Services.ServiceBus;

namespace CommonTestUtilities.ServiceBus;
public class DeleteUserQueueBuilder
{
    public static IDeleteUserQueue Build()
    {
        return new Mock<IDeleteUserQueue>().Object;
    }
}
