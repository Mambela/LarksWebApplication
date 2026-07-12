using LarksWebApp.Models;

namespace LarksWebApp.Repository.DistrictRepository
{
    public interface IDistrictRepository
    {

        DistrictModel VerifyDistrictExist(string designation, Guid countryID);

        bool VerifyDistrictExist(Guid Id);

        Task<DistrictModel> CreateDistrictModel(DistrictModel District);

        DistrictModel GetDistrictModel(Guid DistrictId);

        List<DistrictModel> GetDistrictModels(Guid provinceId);

        public bool DeleteDistrict(DistrictModel District);


        public bool UpdateDistrict(DistrictModel District);

        bool Save();
    }
}
