using Bus.Shared;
using MassTransit;

namespace TwoMicroservice.API.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;


            Console.WriteLine($"Sms Gönderildi, UserId={message.UserId}");

            return Task.CompletedTask;
        }
    }
}