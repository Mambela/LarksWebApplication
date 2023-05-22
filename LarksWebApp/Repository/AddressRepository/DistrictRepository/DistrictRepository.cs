using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.DistrictRepository
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly LarksContext _dbContext;

        public DistrictRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<DistrictModel> CreateDistrictModel(DistrictModel District)
        {
            await _dbContext.Districts.AddAsync(District);

            if (Save()) {
                return District;
            }

            return null;
        }

        public bool DeleteDistrict(DistrictModel District)
        {
            _dbContext.Districts.Remove(District);

            return Save();
        }

        public DistrictModel GetDistrictModel(int DistrictId)
        {
            return _dbContext.Districts.FirstOrDefault(d => d.Id == DistrictId);
        }

        public List<DistrictModel> GetDistrictModels(int provinceId)
        {

            return _dbContext.Districts.Where(d => d.ProvinceId == provinceId).ToList();
            
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateDistrict(DistrictModel District)
        {
            _dbContext.Districts.Update(District);

            return Save();
        }

        public DistrictModel VerifyDistrictExist(string designation, int provinceID)
        {
            return _dbContext.Districts.FirstOrDefault(d => d.Designation.Trim().ToLower() == designation.Trim().ToLower() && d.ProvinceId == provinceID);
        }

        public bool VerifyDistrictExist(int Id)
        {
            return _dbContext.Districts.Any(d => d.Id == Id);
        }
    }
}
