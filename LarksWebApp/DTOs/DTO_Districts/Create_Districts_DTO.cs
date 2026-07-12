namespace LarksWebApp.DTOs.DTO_Districts
{
    public class Create_Districts_DTO
    {

        public Guid Id { get; set; }
        public string Designation { get; set; }

        public Guid ProvinceId { get; set; }
    }
}
