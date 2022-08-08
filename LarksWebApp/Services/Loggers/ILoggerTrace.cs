namespace LarksWebApp.Services.Loggers
{
    public interface ILoggerTrace
    {

        void LogError(string message);
        void Verbose(string message);

        void Error(string message);

        void Fatal(string message);

        void Warning(string message);
    }

}
