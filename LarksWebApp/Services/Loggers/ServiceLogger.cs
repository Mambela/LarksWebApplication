using Serilog;

namespace LarksWebApp.Services.Loggers
{
    public class ServiceLogger : ILoggerTrace
    {
        string fullpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Logs\StdLog.txt");

        private readonly LoggerConfiguration loggerConfiguration;


        public ServiceLogger()
        {
        }

   
        public void Verbose(string message)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(fullpath, rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger.Verbose(message);
        }

        public void Error(string message)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(fullpath, rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger.Error(message);
        }

        public void Fatal(string message)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(fullpath, rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger.Fatal(message);
        }

        public void Warning(string message)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(fullpath, rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger.Warning(message);
        }

        public void LogError(string message)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(fullpath, rollingInterval: RollingInterval.Day).CreateLogger();

            Log.Logger.Error(message);
        }
    }
}
