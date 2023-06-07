using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class SubcategoryModel : TimeStampsEntity
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public CategoryModel Category { get; set; }
        public int CategoryId { get; set; }

        //Language related
        public string Lng { get; set; }

        public IList<LocationModelSubcategoryModel> LocalSubcategories { get; set; }
    }
}
