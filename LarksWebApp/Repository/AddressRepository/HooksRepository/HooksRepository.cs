using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.AddressRepository.HooksRepository
{
    public class HooksRepository:IHooksRepository
    {
        private readonly LarksContext dbContext;

        public HooksRepository(LarksContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<bool> CreateHookAsync(HooksModel hook)
        {
             await dbContext.HooksModel.AddAsync(hook);

            return Save();
        }

        public HooksModel GetHookByDesignation(string designation)
        {
            return dbContext.HooksModel.FirstOrDefault(d => d.Designation.ToLower().Trim() == designation.ToLower().Trim());
        }

        public IList<HooksModel> GetHooksList()
        {
            return  dbContext.HooksModel.ToList(); 
        }

        public IList<HooksModel> GetHooksList(string userId)
        {
            return dbContext.HooksModel.Where(u => u.ApplicationUserId.Trim() == userId.Trim()).ToList();  
        }

        public HooksModel GetHook(int id)
        {
            return dbContext.HooksModel.FirstOrDefault(h => h.Id == id);
        }

        public bool Save()
        {
          return dbContext.SaveChanges() >=0;
        }

        public bool UpdateWebHook(HooksModel model)
        {

            dbContext.HooksModel.Update(model);

            return Save();
        }

        public bool DeleteWebHook(int id)
        {
             dbContext.HooksModel.Remove(GetHook(id));

            return Save();  
        }
    }
}
