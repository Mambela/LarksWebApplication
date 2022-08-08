using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.HoodCoordinateRepository
{
    public interface IHoodCoordinateRepository
    {

        Task<bool> CreateNeighbourhoodCoordinates(IList<NeighbourhoodCoordinates> listStreetCoordinates);

        IList<NeighbourhoodCoordinates> GetNeighbourhoodCoordinates(int streetId);

        bool UpdateCoordiantes(NeighbourhoodCoordinates street);

        bool Save();
    }
}
