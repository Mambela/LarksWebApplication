namespace LarksWebApp.DTOs.DTO_Category
{
    public class SubCategoryDTO
    {

        public int Id { get; set; }
        public string Designation { get; set; }
     
        public int CategoryId { get; set; }

        //Language related
        public string Lng { get; set; }
    }
}
