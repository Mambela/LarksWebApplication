namespace LarksWebApp.DTOs.DTO_Subcategory
{
    public class Create_subcategoryDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public Guid CategoryId { get; set; }

        public string Lng { get; set; }
    }
}
