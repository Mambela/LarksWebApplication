using LarksWebApp.Models;

namespace LarksWebApp.Repository.DistrictRepository
{
    public interface IDistrictRepository
    {

        bool VerifyDistrictExist(string designation, int countryID);

        bool VerifyDistrictExist(int Id);

        Task<DistrictModel> CreateDistrictModel(DistrictModel District);

        DistrictModel GetDistrictModel(int DistrictId);

        List<DistrictModel> GetDistrictModels(int provinceId);

        public bool DeleteDistrict(DistrictModel District);


        public bool UpdateDistrict(DistrictModel District);

        bool Save();
    }
}
