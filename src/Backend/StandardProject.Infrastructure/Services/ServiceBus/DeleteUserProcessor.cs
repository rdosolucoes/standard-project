using Azure.Messaging.ServiceBus;

namespace StandardProject.Infrastructure.Services.ServiceBus;
public class DeleteUserProcessor
{
    private readonly ServiceBusProcessor _processor;

    public DeleteUserProcessor(ServiceBusProcessor processor)
    {
        _processor = processor;
    }

    public ServiceBusProcessor GetProcessor() => _processor;
}
