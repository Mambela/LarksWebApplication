namespace LarksWebApp.DTOs.DTO_Category
{
    public class SubCategoryDTO
    {

        public Guid Id { get; set; }
        public string Designation { get; set; }

        public Guid CategoryId { get; set; }

        //Language related
        public string Lng { get; set; }

        public static implicit operator List<object>(SubCategoryDTO v)
        {
            throw new NotImplementedException();
        }
    }
}
