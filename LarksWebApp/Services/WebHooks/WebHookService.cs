namespace LarksWebApp.Services.WebHooks
{
    public class WebHookService:IWebhookService
    {

        //private readonly IHttpClientFactory _httpClientFactory; 

        private readonly HttpClient _httpClient = new HttpClient();
        //public WebHookService(IHttpClientFactory httpClientFactory)
        //{

        //    _httpClientFactory = httpClientFactory; 
        //}
        public WebHookService()
        {
            
        }

        public void SendWebHook(string endPoint, object data)
        {
           var formattedUrl = endPoint.ToString();

           //var result = _httpClientFactory.CreateClient(formattedUrl).PostAsJsonAsync(formattedUrl,data);

            var result  = _httpClient.PutAsJsonAsync(formattedUrl, data);

            result.Wait();
        }
    }
}
