using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.HoodCoordinateRepository
{
    public interface IHoodCoordinateRepository
    {

        Task<bool> CreateNeighbourhoodCoordinates(IList<SuburbCoordinates> listStreetCoordinates);

        IList<SuburbCoordinates> GetNeighbourhoodCoordinates(Guid streetId);

        bool UpdateCoordiantes(SuburbCoordinates street);

        bool Save();
    }
}
