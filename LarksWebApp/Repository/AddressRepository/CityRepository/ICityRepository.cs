using LarksWebApp.Models;

namespace LarksWebApp.Repository.CityRepository
{
    public interface ICityRepository
    {

        Task<CityModel> CreateCity(CityModel city);

        CityModel GetCity(Guid id);

        CityModel RetrieveCity(Guid id);



        IList<CityModel> GetAllCitiesInProvince(Guid provinceId);

        IList<CityModel> GetAllCitiesInCountry(Guid countryId);

        IList<CityModel> GetAllCities();

        CityModel CityExists(Guid ProvinceId, string designation);

        bool CityExists(Guid cityId);

        bool Save();

        bool UpdateCity(CityModel city);


        bool DeleteCity(CityModel city);

    }
}
