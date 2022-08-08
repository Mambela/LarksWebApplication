using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.CountryRepository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly LarksContext _dbContext;


        public CountryRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
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

        public CountryModel GetCountry(int countryId)
        {

            return _dbContext.Countries.FirstOrDefault(c => c.Id == countryId);
            
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
