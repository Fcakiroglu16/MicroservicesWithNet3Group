namespace TwoMicroservice.API
{
    public class QueueCriticalException(string message) : Exception(message);

    public class QueueNormalException(string message) : Exception(message);
}