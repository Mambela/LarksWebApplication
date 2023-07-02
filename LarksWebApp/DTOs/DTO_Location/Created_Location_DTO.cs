namespace LarksWebApp.DTOs.DTO_Location
{
    public class Created_Location_DTO
    {
        public int Id { get; set; }

        public string Designation { get; set; }

        public int CategoryId { get; set; }

        public int AddressId { get; set; }
        public int ProvinceId { get; set; }

        public int SuburbId { get; set; }

        public int CityId { get; set; }

        public int MunicipalityId { get; set; }

        public decimal Lat  { get; set; }

        public decimal Lng { get; set; }
    }
}
