using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class SubcategoryModel : TimeStampsEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Designation { get; set; }
        public CategoryModel Category { get; set; }
        public Guid CategoryId { get; set; }

        //Language related
        public string Lng { get; set; }

        public IList<LocationModelSubcategoryModel> LocalSubcategories { get; set; }
    }
}
