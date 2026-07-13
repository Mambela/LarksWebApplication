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

        public ProvinceModel GetProvinceModel(Guid ProvinceId)
        {
            return _dbContext.Provinces.Include(n => n.Cities)
                .Include(d =>d.Districts)
                .Include(m =>m.Municipalities)
                .Where(p => p.Id == ProvinceId).FirstOrDefault(p => p.Id == ProvinceId);
        }

        public List<ProvinceModel> GetProvincesInCountry(Guid countryId)
        {
            return _dbContext.Provinces.Where(p => p.CountryId == countryId)
                .Include(m =>m.Municipalities)
                .Include(d =>d.Districts)
                .Include(c =>c.Cities)
                .ToList();
        }

        public List<ProvinceModel> GetProvincesInCountry(string countryName)
        {
            return _dbContext.Provinces.Where(p => p.Country.Designation.ToLower().Trim() == countryName.ToLower().Trim()).ToList();
        }

        public List<ProvinceModel> GetAllProvinces()
        {
            return _dbContext.Provinces.Include(p => p.Country).ToList();
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
 

        public bool VerifyProvinceExist(Guid Id)
        {
            return _dbContext.Provinces.Any(p => p.Id == Id);
        }

        public ProvinceModel VerifyProvinceExist(string designation, Guid countryID)
        {
            return _dbContext.Provinces.FirstOrDefault(p => p.CountryId == countryID && p.Designation.ToLower().Trim() == designation.ToLower().Trim());
        }
    }
}
