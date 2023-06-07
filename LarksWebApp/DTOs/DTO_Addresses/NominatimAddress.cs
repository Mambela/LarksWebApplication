using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class NominatimAddressDTO
    {
        [JsonProperty("place_id")]
        public long PlaceId { get; set; }

        [JsonProperty("licence")]
        public string Licence { get; set; }

        [JsonProperty("osm_type")]
        public string OsmType { get; set; }

        [JsonProperty("osm_id")]
        public long OsmId { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("boundingbox")]
        public string[] Boundingbox { get; set; }

        public string ? HookName { get; set; }
    }

    public class Address
    {
        [JsonProperty("tourism")]
        public string ?Tourism { get; set; }

        [JsonProperty("road")]
        public string ?Road { get; set; }

        [JsonProperty("suburb")]
        public string ?Suburb { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("town")]
        public string? Town { get; set; }

        [JsonProperty("municipality")]
        public string ?Municipality { get; set; }

        [JsonProperty("county")]
        public string? County { get; set; }

        [JsonProperty("city_district")]
        public string ?City_District { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("province")]
        public string? Province { get; set; }

        [JsonProperty("ISO3166-2-lvl4")]
        public string ?Iso31662Lvl4 { get; set; }

        [JsonProperty("country")]
        public string ?Country { get; set; }

        [JsonProperty("country_code")]
        public string ?CountryCode { get; set; }
    }

    //public partial class NominatimAddressDTO
    //{
    //    public static NominatimAddressDTO FromJson(string json) => JsonConvert.DeserializeObject<NominatimAddressDTO>(json, Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this NominatimAddressDTO self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
