using MassTransit;
using TwoMicroservice.API;
using TwoMicroservice.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();


    x.UsingRabbitMq((context, configure) =>
    {
        configure.UseMessageRetry(r =>
        {
            r.Interval(5, TimeSpan.FromSeconds(10));

            r.Handle<QueueCriticalException>();
            r.Ignore<QueueNormalException>();
        });


        //configure.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

        configure.PrefetchCount = 10;
        configure.ConcurrentMessageLimit = 5;

        configure.UseDelayedRedelivery(x => x.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(2)));

        configure.UseInMemoryOutbox(context);

        var connectionString = builder.Configuration.GetConnectionString("RabbitMQ");
        configure.Host(connectionString);


        // microservice.queueName.document
        configure.ReceiveEndpoint("email-microservice.user-created-event.queue",
            e => { e.ConfigureConsumer<UserCreatedEventConsumer>(context); });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();