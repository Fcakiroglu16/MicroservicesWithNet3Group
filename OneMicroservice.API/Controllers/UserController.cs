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
            //  user to create;


            await publishEndpoint.Publish(new UserCreatedEvent(Guid.NewGuid(), "ahmet@outlook.com", "555 555 55 55"));
            return Ok();
        }
    }
}