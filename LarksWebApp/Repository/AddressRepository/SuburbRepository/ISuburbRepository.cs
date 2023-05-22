using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public interface ISuburbRepository
    {
        SuburbModel VerifySuburbExist(string designation, int municipalityId,int cityId=0, int districtId=0);

        SuburbModel GetSuburbInMunicipality(string designation, string municipalityId);

        bool VerifyNeighbourhoodExist(int Id);

        Task<SuburbModel> CreateSuburb(SuburbModel neighbourhood);

        SuburbModel GetNeighbourhood(int neighbourhoodId);

        IList<SuburbModel> GetNeighbourhoodInMunicipality(int provinceId);
        
        
        IList<SuburbModel> GetNeighbourhoodModels();

        IList<SuburbModel> GetNeighbourhoodModels(string sector, int id);


        IList<SuburbModel> GetNeighbourhoodInDistricts(int districtiD);
     
        IList<SuburbModel> GetNeighbourhoodInCities(int cityID);


        public bool DeleteNeighbourhood(SuburbModel neighbourhood);


        public bool UpdateNeighbourhoody(SuburbModel neighbourhood);

        bool Save();
    }
}
