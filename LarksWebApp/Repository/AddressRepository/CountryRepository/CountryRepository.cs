using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.CountryRepository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly LarksContext _dbContext;


        public CountryRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CountryModel CountryExistis(string designation)
        {
            return _dbContext.Countries.Include(p => p.Provinces).Where(c => c.Designation.ToLower() == designation.ToLower()).FirstOrDefault();
        }

        public bool CountryExists(string designation)
        {

            bool result = _dbContext.Countries.Any(c => c.Designation.Trim().ToLower() == designation.ToLower().Trim());
            return result;
        }

        public bool CountryExists(int countryID)
        {
            return _dbContext.Countries.Any(c => c.Id == countryID);
        }

        public async Task<CountryModel> CreateCountry(CountryModel country)
        {

            await _dbContext.Countries.AddAsync(country);

            var result = Save();

            if (result)
            {
                return country;
            }


            return null;
            
        }

        public bool DeleteCountry(CountryModel country)
        {
            _dbContext.Countries.Remove(country);

            return Save();
        }

        public IList<CountryModel> GetAll()
        {
            return _dbContext.Countries.OrderBy(c =>c.Designation).ToList();
        }

        public IList<CountryModel> GetCountries(int countryID)
        {
            return _dbContext.Countries.Include(p => p.Provinces).Where(c => c.Id == countryID).ToList();
        }

        public CountryModel GetCountry(int countryId)
        {
           return _dbContext.Countries.Include(p => p.Provinces).Where(c => c.Id == countryId).FirstOrDefault();
          
           }



        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCountry(CountryModel Country)
        {
            _dbContext.Countries.Update(Country);


            return Save();
        }
    }
}
