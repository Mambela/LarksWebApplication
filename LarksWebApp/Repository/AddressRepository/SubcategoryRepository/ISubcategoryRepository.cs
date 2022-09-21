using LarksWebApp.Models;

namespace LarksWebApp.Repository.SubcategoryRepository
{
    public interface ISubcategoryRepository
    {
        /*
         * Get all the subCategories under a category
         * 
         */
        public Task<SubcategoryModel> CreateSubCategory(SubcategoryModel subcategory);

        public bool SubCategoryExists(string subcategory);
        public bool SubCategoryExists(int subcategoryId);

        public IList<SubcategoryModel> GetSubcategories(List<int> listCategoryIds);

        public IList<SubcategoryModel> GetSubcategoriesInCategory(int categoryId);

        public SubcategoryModel GetCategoryById(int id);

        public IList<SubcategoryModel> GetAllSubCategories();

        public bool DeleteSubCategory(SubcategoryModel subcategory);

        public bool UpdateSubCategory(SubcategoryModel subcategory);

        public bool Save();
    }
}
