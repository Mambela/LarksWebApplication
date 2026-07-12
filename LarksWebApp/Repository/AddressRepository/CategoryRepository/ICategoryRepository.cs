using LarksWebApp.Models;

namespace LarksWebApp.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {

        public Task<CategoryModel> CreateCategory(CategoryModel category);

        public bool CategoryExists(string designation);
        public bool CategoryExists(Guid categoryId);

        public CategoryModel GetCategoryById(Guid id);

        public IList<CategoryModel> GetAllCategories(string lang=null);

        public bool DeleteCategory(CategoryModel category);

        public bool UpdateCategory(CategoryModel category);

        public bool Save();
    }
}
