using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.CityRepository
{
    public class CityRepository : ICityRepository
    {
        private readonly LarksContext _dbContext;
        public CityRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CityExists(int ProvinceId, string designation)
        {
            return _dbContext.Cities.Any(p => p.ProvinceId == ProvinceId && p.Designation.Trim().ToLower() == designation.Trim().ToLower()); 
        }

        public bool CityExists(int cityId)
        {
            return _dbContext.Cities.Any(c => c.Id == cityId);
        }

        public async Task<CityModel> CreateCity(CityModel city)
        {

            await _dbContext.Cities.AddAsync(city);

            if (Save())
            {
                return city;
            }

            return null;
        }

        public bool DeleteCity(CityModel city)
        {
            _dbContext.Cities.Remove(city);

            return Save();
        }

        public IList<CityModel> GetAllCitiesInProvince(int provinceId)
        {
            return _dbContext.Cities.Where(p => p.ProvinceId == provinceId).ToList();
        }

        public CityModel GetCity(int id)
        {
            return _dbContext.Cities.Include(c => c.Neighbourhoods.Where(n => n.Id == id)).FirstOrDefault();   
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCity(CityModel city)
        {
            _dbContext.Cities.Update(city);

            return Save();
        }
    }
}
