using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.ProviceRepository
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly LarksContext _dbContext;

        public ProvinceRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProvinceModel> CreateProvinceModel(ProvinceModel Province)
        {

            await _dbContext.Provinces.AddAsync(Province);

            if (Save()) {

                return Province;
            }

            return null;
        }

        public bool DeleteProvince(ProvinceModel Province)
        {
            _dbContext.Provinces.Remove(Province);

            return Save();
        }

        public ProvinceModel GetProvinceModel(int ProvinceId)
        {
            return _dbContext.Provinces.Include(n => n.Cities)
                .Include(d =>d.Districts)
                .Include(m =>m.Municipalities)
                .Where(p => p.Id == ProvinceId).FirstOrDefault(p => p.Id == ProvinceId);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >=0; 
        }

        public bool UpdateProvince(ProvinceModel Province)
        {
            _dbContext.Provinces.Update(Province);
            
            return Save();
        }

        public bool VerifyProvinceExist(string designation, int countryID)
        {
            return _dbContext.Provinces.Any(p => p.Designation.ToLower().Trim() == designation.ToLower().Trim() && p.CountryId == countryID);
        }

        public bool VerifyProvinceExist(int Id)
        {
            return _dbContext.Provinces.Any(p => p.Id == Id);
        }
    }
}
