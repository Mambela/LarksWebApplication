using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.SuburbCoordinateRepository
{
    public interface ISuburbCoordinateRepository
    {

        Task<bool> CreateSuburbCoordinates(IList<SuburbCoordinates> listStreetCoordinates);

        IList<SuburbCoordinates> GetSuburbCoordinates(Guid suburbId);

        bool UpdateCoordiantes(SuburbCoordinates street);

        bool Save();
    }
}
