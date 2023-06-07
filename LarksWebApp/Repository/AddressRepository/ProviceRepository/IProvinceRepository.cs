using LarksWebApp.Models;

namespace LarksWebApp.Repository.ProviceRepository
{
    public interface IProvinceRepository
    {
        
        ProvinceModel VerifyProvinceExist(string designation,int countryID);

        bool VerifyProvinceExist(int Id);

        Task<ProvinceModel> CreateProvinceModel(ProvinceModel Province);

        ProvinceModel GetProvinceModel(int ProvinceId);

        List<ProvinceModel> GetProvincesInCountry(int countryId);
        List<ProvinceModel> GetProvincesInCountry(string countryName);

        public bool DeleteProvince(ProvinceModel Province);


        public bool UpdateProvince(ProvinceModel Province);

        bool Save();
    }
}
