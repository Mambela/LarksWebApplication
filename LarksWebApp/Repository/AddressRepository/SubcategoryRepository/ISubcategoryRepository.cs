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
        public bool SubCategoryExists(Guid subcategoryId);

        public IList<SubcategoryModel> GetSubcategories(List<Guid> listCategoryIds);

        public IList<SubcategoryModel> GetSubcategoriesInCategory(Guid categoryId);

        public SubcategoryModel GetSubcateogryById(Guid id);

        public IList<SubcategoryModel> GetAllSubCategories(string lang);

        public bool DeleteSubCategory(SubcategoryModel subcategory);

        public bool UpdateSubCategory(SubcategoryModel subcategory);

        public bool Save();
    }
}
