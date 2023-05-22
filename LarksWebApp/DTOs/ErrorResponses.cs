namespace LarksWebApp.DTOs
{
    public class ErrorResponses
    {

        public IList<ErrorMessage> ErrorMessages { get; set; }

        public string MainMessage { get; set; }
    }

    public class ErrorMessage
    {
        public string Source { get; set; }

        public string Message { get; set; }
    }
}
