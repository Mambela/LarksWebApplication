using LarksWebApp.Models;

namespace LarksWebApp.Repository.SuburbRepository
{
    public interface ISuburbRepository
    {
        SuburbModel VerifySuburbExist(string designation, Guid? municipalityId,Guid? cityId=null, Guid? districtId=null);

        SuburbModel GetSuburbInMunicipality(string designation, string municipalityId);

        bool VerifySuburbExistsById(Guid Id);

        Task<SuburbModel> CreateSuburb(SuburbModel suburb);

        SuburbModel GetSuburb(Guid suburbId);

        IList<SuburbModel> GetSuburbsByName(string designation);

        IList<SuburbModel> GetSuburbsInMunicipality(Guid municipalityId);


        IList<SuburbModel> GetSuburbs();

        IList<SuburbModel> GetSuburbs(string sector, Guid id);
        IList<SuburbModel> GetSuburbs(Guid districtId, Guid municipalityId,Guid cityId);

        IList<SuburbModel> GetSuburbsInProvince(Guid ProvinceId);


        IList<SuburbModel> GetSuburbInDistricts(Guid districtiD);

        IList<SuburbModel> GetSuburbsInCities(Guid cityID);

        IList<SuburbModel> GetSuburbs(IList<Guid> suburbs);


        public bool DeleteSuburb(SuburbModel suburb);


        public bool UpdateSuburb(SuburbModel suburb);

        bool Save();
    }
}
