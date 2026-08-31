namespace LarksWebApp.DTOs.DTO_Location
{
    public class Created_Location_DTO
    {
        public Guid Id { get; set; }

        public string Designation { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ? AddressId { get; set; }
        public Guid ProvinceId { get; set; }

        public Guid SuburbId { get; set; }

        public Guid CityId { get; set; }

        public Guid MunicipalityId { get; set; }

        public decimal Lat  { get; set; }

        public decimal Lng { get; set; }
    }
}
