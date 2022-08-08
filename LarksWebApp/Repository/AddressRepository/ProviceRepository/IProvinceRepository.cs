using LarksWebApp.Models;

namespace LarksWebApp.Repository.ProviceRepository
{
    public interface IProvinceRepository
    {
        bool VerifyProvinceExist(string designation,int countryID);

        bool VerifyProvinceExist(int Id);

        Task<ProvinceModel> CreateProvinceModel(ProvinceModel Province);

        ProvinceModel GetProvinceModel(int ProvinceId);

        public bool DeleteProvince(ProvinceModel Province);


        public bool UpdateProvince(ProvinceModel Province);

        bool Save();
    }
}
