namespace LarksWebApp.DTOs.DTO_Neighbourhoods
{
    public class SuburbDTO
    {
        public Guid Id { get; set; }
        public Guid MunicipalityId { get; set; }
        public string Designation { get; set; }

        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string MunicipalityName { get; set; }
        public string ProvinceName { get; set; }
    }
}
