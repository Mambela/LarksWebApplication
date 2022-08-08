using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class CategoryModel :TimeStampsEntity
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public IList<SubcategoryModel> Subcategories { get; set; }

        public string Lng { get; set; }
    }
}
