namespace Production.Abstract
{
    public class NullableLogger : ILogger
    {
        private readonly ILogger logger;
        public NullableLogger(ILogger logger = null)
        {
            this.logger = logger;
        }
        public void LogDebug(string message)
        {
            if (logger != null)
            {
                logger.LogDebug(message);
            }
        }
        public void LogError(string message)
        {
            if (logger != null)
            {
                logger.LogError(message);
            }
        }
    }
}
