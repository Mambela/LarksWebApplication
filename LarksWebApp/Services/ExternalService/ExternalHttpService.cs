using LarksWebApp.DTOs.DTO_Addresses;
using Newtonsoft.Json;

namespace LarksWebApp.Services.ExternalService
{
    public class ExternalHttpService
    {
        public readonly HttpClient httpClient = new HttpClient();
        public readonly IConfiguration config;
        
        
        public ExternalHttpService(IConfiguration _config)
        {
            config = _config;
        }

        public async Task<NominatimAddressDTO> RetrieveLocationByGeoLocationAsync(decimal lat,decimal lng) 
        {
            string endPoint = config["Endpoints:NominatimReverse"].ToString();

            var formmatterUrl = string.Format(endPoint, lat, lng);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

            var result = await httpClient.GetAsync(formmatterUrl);

            result.EnsureSuccessStatusCode();   

            var responseAsString = await result.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<NominatimAddressDTO>(value: responseAsString);

        }


        public async Task<object> ApiRequest(object ?payload,string endpoint, string method)
        {
            switch (method.ToLower().Trim())
            {                   
                case "post":
                        return await Post(payload, endpoint);
                     
                case "put":
                        return await Put(payload, endpoint);
                case "patch":
                    return await Patch(payload, endpoint);

                default:
                        return null;
            }
        }
        private async Task<object> Post(object? payload, string endpoint)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

                var result = await httpClient.PostAsJsonAsync(endpoint,payload);

                result.EnsureSuccessStatusCode();

                var responseAsString = await result.Content.ReadAsStringAsync();


                return JsonConvert.DeserializeObject<object>(value: responseAsString);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<object> Patch(object? payload, string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);

                var result = await httpClient.PutAsJsonAsync(endpoint, payload);

                result.EnsureSuccessStatusCode();

                var responseAsString = await result.Content.ReadAsStringAsync();


                return JsonConvert.DeserializeObject<object>(value: responseAsString);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task<object> Put(object? payload, string endpoint)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

                var result = await httpClient.PutAsJsonAsync(endpoint, payload);

                result.EnsureSuccessStatusCode();

                var responseAsString = await result.Content.ReadAsStringAsync();


                return JsonConvert.DeserializeObject<object>(value: responseAsString);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
