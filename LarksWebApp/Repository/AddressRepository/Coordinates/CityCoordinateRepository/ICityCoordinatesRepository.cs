using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.CityCoordinateRepository
{
    public interface ICityCoordinatesRepository
    {

        Task<bool> CreateCityCoordinates(IList<CityCoordinates> listCityCoordinates);

        IList<CityCoordinates> GetCityCoordinates(int streetId);

        bool UpdateCoordiantes(CityCoordinates street);

        bool Save();
    }
}
