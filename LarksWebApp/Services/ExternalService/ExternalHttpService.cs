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

    }
}
