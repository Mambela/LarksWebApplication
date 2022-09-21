using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.SubcategoryRepository
{
    public class SubcategoryRepository:ISubcategoryRepository
    {
        private readonly LarksContext _dbContext;

        public SubcategoryRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool SubCategoryExists(string subcategory)
        {
            return _dbContext.Subcategories.Any(s => s.Designation.Trim().ToLower() == subcategory.Trim().ToLower());
        }

        public async Task<SubcategoryModel> CreateSubCategory(SubcategoryModel subcategory)
        {
            await _dbContext.Subcategories.AddAsync(subcategory);

            var result = Save();

            if (result)
            {
                return subcategory;
            }

            return null;
        }

        public bool DeleteSubCategory(SubcategoryModel subcategory)
        {

            _dbContext.Subcategories.Remove(subcategory);

            return Save();
        }

        public IList<SubcategoryModel> GetAllSubCategories()
        {
            return _dbContext.Subcategories.ToList();
        }

        public SubcategoryModel GetCategoryById(int id)
        {
            return _dbContext.Subcategories.FirstOrDefault(s => s.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateSubCategory(SubcategoryModel subcategory)
        {
            _dbContext.Subcategories.Update(subcategory);

            return Save();
        }

        public bool SubCategoryExists(int subcategoryId)
        {
            return _dbContext.Subcategories.Any(s => s.Id == subcategoryId);
        }

        public IList<SubcategoryModel> GetSubcategories(List<int> listCategoryIds)
        {
            var subcategories = _dbContext.Subcategories.Where(l => listCategoryIds.Where(c =>c == l.Id).Any()).ToList();

            return subcategories;
        }

        public IList<SubcategoryModel> GetSubcategoriesInCategory(int categoryId)
        {
            return _dbContext.Subcategories.Where(l => l.CategoryId == categoryId).ToList();
        }
    }
}
