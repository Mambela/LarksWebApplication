using LarksWebApp.Model;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.CountryRepository
{
    public interface ICountryRepository
    {
        bool CountryExists(string designation);
        bool CountryExists(Guid countryID);

        CountryModel CountryExistis(string designation);
        CountryModel CountryandDivisions(string designation);

        Task<CountryModel> CreateCountry(CountryModel Country);

        CountryModel GetCountry(Guid countryId);

        IList<CountryModel> GetCountries(Guid countryID);

        IList<CountryModel> GetAll();

        public bool DeleteCountry(CountryModel country);


        public bool UpdateCountry(CountryModel Country);

        bool Save();
    }
}
