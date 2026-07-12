using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.MunicipalityCoordinateRepository
{
    public interface IMunicipalityCoordinateRepository
    {

        Task<bool> CreateMunicipalityCoordinates(IList<MunicipalityCoordinates> listMunicipalityCoordinates);

        IList<MunicipalityCoordinates> GetMunicipalityCoordinates(Guid municipalityId);

        bool UpdateCoordiantes(MunicipalityCoordinates municipalityList);

        bool Save();
    }
}
