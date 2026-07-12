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

        public CityModel CityExists(Guid ProvinceId, string designation)
        {
            return _dbContext.Cities.FirstOrDefault(p => p.ProvinceId == ProvinceId && p.Designation.Trim().ToLower() == designation.Trim().ToLower());
        }

        public bool CityExists(Guid cityId)
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

        public IList<CityModel> GetAllCitiesInCountry(Guid countryId)
        {
            return _dbContext.Cities.Where(c =>c.Province.CountryId == countryId).ToList();
        }

        public IList<CityModel> GetAllCitiesInProvince(Guid provinceId)
        {
            return _dbContext.Cities.Where(p => p.ProvinceId == provinceId).ToList();
        }

        public CityModel GetCity(Guid id)
        {
            return _dbContext.Cities.Include(c => c.Neighbourhoods.Where(n => n.Id == id)).FirstOrDefault();
        }

        public CityModel RetrieveCity(Guid id)
        {
            return _dbContext.Cities.FirstOrDefault(c => c.Id == id);
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
