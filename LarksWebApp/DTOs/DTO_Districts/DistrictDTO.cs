namespace LarksWebApp.DTOs.DTO_Districts
{
    public class DistrictDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public Guid ProvinceId { get; set; }
    }
}
