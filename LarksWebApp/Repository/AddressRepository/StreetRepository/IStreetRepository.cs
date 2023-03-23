using LarksWebApp.Models;

namespace LarksWebApp.Repository.StreetRepository
{
    public interface IStreetRepository
    {

        bool VerifyStreetExist(string designation);

        bool VerifyStreetExist(int Id);

        Task<StreetModel> CreateStreetModel(StreetModel Street);

        List<NeighbourhoodStreets> GetStreetInNeighBourhood(int neighbourhoodId);

        List<NeighbourhoodStreets> GetStreetInNeighBourhoodByValue(int neighbourhoodId,string hood);

        StreetModel GetStreetModel(int StreetId);

        public bool DeleteStreet(StreetModel street);


        public bool UpdateStreet(StreetModel street);

        bool Save();
    }
}
