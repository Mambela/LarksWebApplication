using LarksWebApp.Models;

namespace LarksWebApp.Repository.StreetRepository
{
    public interface IStreetRepository
    {

        bool VerifyStreetExist(string designation);
        StreetModel VerifyStreetExistance(string designation);

        bool VerifyStreetExist(int Id);

        bool VerifyStreetSuburbLink(int streetsId, int suburbId);

        Task<StreetModel> CreateStreetModel(StreetModel Street);

        Task<bool> CreateSuburbStreetsLink(SuburbStreets suburbStreets);

        List<SuburbStreets> GetStreetInNeighBourhood(int neighbourhoodId);

        List<SuburbStreets> GetStreetInNeighBourhoodByValue(int neighbourhoodId,string hood);

        StreetModel GetStreetModel(int StreetId);

        public bool DeleteStreet(StreetModel street);


        public bool UpdateStreet(StreetModel street);

        bool Save();
    }
}
