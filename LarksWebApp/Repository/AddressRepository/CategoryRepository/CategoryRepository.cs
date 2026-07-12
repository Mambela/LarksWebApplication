using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LarksContext _dbContext;
        public CategoryRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CategoryExists(string categoryDesignation)
        {
            return _dbContext.Categories.Any(c => c.Designation.ToLower().Trim() == categoryDesignation.ToLower().Trim());
        }

        public bool CategoryExists(Guid categoryId)
        {
            return _dbContext.Categories.Any(c => c.Id == categoryId);   
        }

        public async Task<CategoryModel> CreateCategory(CategoryModel category)
        {
            await _dbContext.Categories.AddAsync(category);

            var result = Save();

            if (result)
            {
                return category;
            }
            

            return null;
        }

        public bool DeleteCategory(CategoryModel category)
        {

            _dbContext.Categories.Remove(category);   

            return Save();
            
        }

        public IList<CategoryModel> GetAllCategories(string lng=null)
        {
            if(lng != null)
            { 
                return _dbContext.Categories
                .Where(c =>c.Lng.ToLower().Trim() == lng.ToLower().Trim())
                .ToList();
            }

            return _dbContext.Categories
            .ToList();

        }

        public CategoryModel GetCategoryById(Guid id)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >=0;
        }

        public bool UpdateCategory(CategoryModel category)
        {
            _dbContext.Categories.Update(category);

            return Save();
        }
    }
}
