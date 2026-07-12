using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public interface ISuburbRepository
    {
        SuburbModel VerifySuburbExist(string designation, Guid? municipalityId,Guid? cityId=null, Guid? districtId=null);

        SuburbModel GetSuburbInMunicipality(string designation, string municipalityId);

        bool VerifyNeighbourhoodExist(Guid Id);

        Task<SuburbModel> CreateSuburb(SuburbModel neighbourhood);

        SuburbModel GetSuburb(Guid neighbourhoodId);

        IList<SuburbModel> GetSuburbsByName(string designation);

        IList<SuburbModel> GetNeighbourhoodInMunicipality(Guid municipalityId);


        IList<SuburbModel> GetNeighbourhoodModels();

        IList<SuburbModel> GetNeighbourhoodModels(string sector, Guid id);
        IList<SuburbModel> GetNeighbourhoodModels(Guid districtId, Guid municipalityId,Guid cityId);

        IList<SuburbModel> GetSuburbsInProvince(Guid ProvinceId);


        IList<SuburbModel> GetSuburbInDistricts(Guid districtiD);

        IList<SuburbModel> GetSuburbsInCities(Guid cityID);

        IList<SuburbModel> GetSuburbs(IList<Guid> suburbs);


        public bool DeleteNeighbourhood(SuburbModel neighbourhood);


        public bool UpdateNeighbourhoody(SuburbModel neighbourhood);

        bool Save();
    }
}
