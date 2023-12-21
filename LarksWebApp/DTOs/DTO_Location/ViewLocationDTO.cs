using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class ViewLocationDTO
    {

        public int Id { get; set; }
        public string Designation { get; set; }
        public string AdditionalInformation { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Municipality { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public decimal? Distance { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public IList<SubCategoryDTO> Subcategories { get; set; }
        public IList<string> Images { get; set; }
    }
}
