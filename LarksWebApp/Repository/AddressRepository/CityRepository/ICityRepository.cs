using LarksWebApp.Models;

namespace LarksWebApp.Repository.CityRepository
{
    public interface ICityRepository
    {

        Task<CityModel> CreateCity(CityModel city);

        CityModel GetCity(int id);

        CityModel RetrieveCity(int id); 



        IList<CityModel> GetAllCitiesInProvince(int provinceId);  

        IList<CityModel> GetAllCitiesInCountry(int countryId);

        CityModel CityExists(int ProvinceId, string designation);

        bool CityExists(int cityId);

        bool Save();

        bool UpdateCity(CityModel city);


        bool DeleteCity(CityModel city);

    }
}
