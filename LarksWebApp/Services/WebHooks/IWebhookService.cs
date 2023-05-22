namespace LarksWebApp.Services.WebHooks
{
    public interface IWebhookService
    {

        //Send Data to a client. 

        void SendWebHook(string endPoint, object data);
        


        //
    }
}
