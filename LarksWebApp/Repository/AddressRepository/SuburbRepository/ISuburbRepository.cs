using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public interface ISuburbRepository
    {
        SuburbModel VerifySuburbExist(string designation, int municipalityId,int cityId=0, int districtId=0);

        SuburbModel GetSuburbInMunicipality(string designation, string municipalityId);

        bool VerifyNeighbourhoodExist(int Id);

        Task<SuburbModel> CreateSuburb(SuburbModel neighbourhood);

        SuburbModel GetSuburb(int neighbourhoodId);

        IList<SuburbModel> GetSuburbsByName(string designation);

        IList<SuburbModel> GetNeighbourhoodInMunicipality(int municipalityId);
        
        
        IList<SuburbModel> GetNeighbourhoodModels();

        IList<SuburbModel> GetNeighbourhoodModels(string sector, int id);
        IList<SuburbModel> GetNeighbourhoodModels(int districtId, int municipalityId,int cityId);

        IList<SuburbModel> GetSuburbsInProvince(int ProvinceId);


        IList<SuburbModel> GetSuburbInDistricts(int districtiD);
     
        IList<SuburbModel> GetSuburbsInCities(int cityID);

        IList<SuburbModel> GetSuburbs(IList<int> suburbs);


        public bool DeleteNeighbourhood(SuburbModel neighbourhood);


        public bool UpdateNeighbourhoody(SuburbModel neighbourhood);

        bool Save();
    }
}
