using Bus.Shared;
using MassTransit;

namespace TwoMicroservice.API.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            // transaction

            // queue

            // transaction

            // queue


            Console.WriteLine("Consume methodu çalıştı");


            throw new Exception("hata meydana geldi");

            var message = context.Message;


            Console.WriteLine($"Sms Gönderildi, UserId={message.UserId}");

            return Task.CompletedTask;
        }
    }
}