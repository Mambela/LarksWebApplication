namespace LarksWebApp.Models
{
    public class LocationModelSubcategoryModel
    {
        public LocationModel Location { get; set; }
        public int LocationId { get; set; }


        public SubcategoryModel Subcategories { get; set; }
        public int SubcategoriesId { get; set; }
    }
}
