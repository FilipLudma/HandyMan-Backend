namespace WebAPI.Logging
{
    using Microsoft.Extensions.Logging;

    public class ApplicationLogging
    {
        private static ILoggerFactory _Factory = null;

        public static void ConfigureLogger(ILoggerFactory factory)
        {
            // factory.AddConsole(Configuration.GetSection("Logging"));
            factory.AddDebug(LogLevel.None);
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    ConfigureLogger(_Factory);
                }
                return _Factory;
            }
            set { _Factory = value; }
        }
        public static ILogger CreateLogger() => LoggerFactory.CreateLogger("Values");
    }
}