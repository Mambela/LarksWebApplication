using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public interface INeighbourhoodRepository
    {
        bool VerifyNeighbourhoodExist(string designation, int municipalityId);

        bool VerifyNeighbourhoodExist(int Id);

        Task<NeighbourhoodModel> CreateNeighbourhood(NeighbourhoodModel neighbourhood);

        NeighbourhoodModel GetNeighbourhood(int neighbourhoodId);

        IList<NeighbourhoodModel> GetNeighbourhoodInMunicipality(int provinceId);
        
        
        IList<NeighbourhoodModel> GetNeighbourhoodModels();

        IList<NeighbourhoodModel> GetNeighbourhoodModels(string sector, int id);


        IList<NeighbourhoodModel> GetNeighbourhoodInDistricts(int districtiD);
     
        IList<NeighbourhoodModel> GetNeighbourhoodInCities(int cityID);


        public bool DeleteNeighbourhood(NeighbourhoodModel neighbourhood);


        public bool UpdateNeighbourhoody(NeighbourhoodModel neighbourhood);

        bool Save();
    }
}
