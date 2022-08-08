using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.MunicipalityCoordinateRepository
{
    public interface IMunicipalityCoordinateRepository
    {

        Task<bool> CreateMunicipalityCoordinates(IList<MunicipalityCoordinates> listMunicipalityCoordinates);

        IList<MunicipalityCoordinates> GetMunicipalityCoordinates(int municipalityId);

        bool UpdateCoordiantes(MunicipalityCoordinates municipalityList);

        bool Save();
    }
}
