using LarksWebApp.Model;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.CountryRepository
{
    public interface ICountryRepository
    {
        bool CountryExists(string designation);
        bool CountryExists(int countryID);

        Task<CountryModel> CreateCountry(CountryModel Country);

        CountryModel GetCountry(int countryId);

        public bool DeleteCountry(CountryModel country);


        public bool UpdateCountry(CountryModel Country);

        bool Save();
    }
}
