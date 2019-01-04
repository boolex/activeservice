namespace Production.Abstract
{
    public interface ILogger
    {
        void LogError(string message);
        void LogDebug(string message);
    }
}
