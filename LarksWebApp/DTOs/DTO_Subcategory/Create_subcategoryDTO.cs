namespace LarksWebApp.DTOs.DTO_Subcategory
{
    public class Create_subcategoryDTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public int CategoryId { get; set; }

        public string Lng { get; set; }
    }
}
