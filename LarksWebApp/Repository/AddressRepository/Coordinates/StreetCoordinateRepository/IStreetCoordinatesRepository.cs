using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.StreetCoordinateRepository
{
    public interface IStreetCoordinatesRepository
    {

        Task<bool> CreateStreetCoordinates(IList<StreetCoordinates> streetCoordinates);

        IList<StreetCoordinates> GetStreetCoordinates(int streetId);

        bool UpdateCoordiantes(StreetCoordinates street);

        bool Save();

    }
}
