using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.ProviceCoordinateRepository
{
    public interface IProvinceCoordinatesRepository
    {

        Task<bool> CreateProvinceCoordinates(IList<ProvinceCoordinates> listStreetCoordinates);

        IList<ProvinceCoordinates> GetProvinceCoordinates(int provinceId);

        bool UpdateCoordiantes(ProvinceCoordinates province);

        bool Save();
    }
}
