namespace LarksWebApp.DTOs.DTO_Municipalities
{
    public class MunicipalityDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }
}
