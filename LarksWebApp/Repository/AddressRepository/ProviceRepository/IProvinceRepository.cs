using LarksWebApp.Models;

namespace LarksWebApp.Repository.ProviceRepository
{
    public interface IProvinceRepository
    {
        
        ProvinceModel VerifyProvinceExist(string designation,Guid countryID);

        bool VerifyProvinceExist(Guid Id);

        Task<ProvinceModel> CreateProvinceModel(ProvinceModel Province);

        ProvinceModel GetProvinceModel(Guid ProvinceId);

        List<ProvinceModel> GetProvincesInCountry(Guid countryId);
        List<ProvinceModel> GetProvincesInCountry(string countryName);

        List<ProvinceModel> GetAllProvinces();

        public bool DeleteProvince(ProvinceModel Province);


        public bool UpdateProvince(ProvinceModel Province);

        bool Save();
    }
}
