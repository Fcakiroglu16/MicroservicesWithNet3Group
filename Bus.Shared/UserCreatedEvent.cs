namespace Bus.Shared
{
    public record UserCreatedEvent(Guid UserId, string Email, string Phone);
}