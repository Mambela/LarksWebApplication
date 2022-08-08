using LarksWebApp.Models;

namespace LarksWebApp.Repository.CityRepository
{
    public interface ICityRepository
    {

        Task<CityModel> CreateCity(CityModel city);

        CityModel GetCity(int id);



        IList<CityModel> GetAllCitiesInProvince(int provinceId);  

        bool CityExists(int ProvinceId, string designation);

        bool CityExists(int cityId);

        bool Save();

        bool UpdateCity(CityModel city);


        bool DeleteCity(CityModel city);

    }
}
