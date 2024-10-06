using Bus.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            //Outbox design
            //Inbox  design

            //transaction begin
            //  user to create; Sql server
            //Outbox(created,message payload, status)
            //transaction end


            // Retry =>  count(5),timeout
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(60));


            await publishEndpoint.Publish(new UserCreatedEvent(Guid.NewGuid(), "ahmet@outlook.com", "555 555 55 55"),
                pipeline =>
                {
                    pipeline.SetAwaitAck(true);
                    pipeline.SetAwaitAck(true);
                    pipeline.Durable = true;
                    pipeline.TimeToLive = TimeSpan.FromMinutes(1);
                }, cancellationTokenSource.Token);
            return Ok();
        }
    }
}