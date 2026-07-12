using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class CategoryModel :TimeStampsEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Designation { get; set; }
        public IList<SubcategoryModel> Subcategories { get; set; }

        //Language related
        public string Lng { get; set; }
    }
}
