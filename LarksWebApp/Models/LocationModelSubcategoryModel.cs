namespace LarksWebApp.Models
{
    public class LocationModelSubcategoryModel
    {
        public LocationModel Location { get; set; }
        public Guid LocationId { get; set; }


        public SubcategoryModel Subcategories { get; set; }
        public Guid SubcategoriesId { get; set; }
    }
}
