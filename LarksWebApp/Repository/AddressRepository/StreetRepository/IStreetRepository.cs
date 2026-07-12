using LarksWebApp.Models;

namespace LarksWebApp.Repository.StreetRepository
{
    public interface IStreetRepository
    {

        bool VerifyStreetExist(string designation);
        StreetModel VerifyStreetExistance(string designation);

        bool VerifyStreetExist(Guid Id);

        bool VerifyStreetSuburbLink(Guid streetsId, Guid suburbId);

        Task<StreetModel> CreateStreetModel(StreetModel Street);

        Task<bool> CreateSuburbStreetsLink(SuburbStreets suburbStreets);

        List<SuburbStreets> GetStreetInNeighBourhood(Guid neighbourhoodId);

        List<SuburbStreets> GetStreetInNeighBourhoodByValue(Guid neighbourhoodId,string hood);

        StreetModel GetStreetModel(Guid StreetId);

        public bool DeleteStreet(StreetModel street);


        public bool UpdateStreet(StreetModel street);

        bool Save();
    }
}
