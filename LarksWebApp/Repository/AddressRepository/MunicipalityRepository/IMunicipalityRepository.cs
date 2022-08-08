using LarksWebApp.Models;

namespace LarksWebApp.Repository.MunicipalityRepository
{
    public interface IMunicipalityRepository
    {

        bool VerifyMunicipalityExist(string designation,int provinceId);

        bool VerifyMunicipalityExist(int Id);

        Task<MunicipalityModel> CreateProvinceModel(MunicipalityModel municipality);

        MunicipalityModel GetMuncipality(int ProvinceId);

        public bool DeleteMunicipality(MunicipalityModel municipality);


        public bool UpdateMunicipality(MunicipalityModel municipality);

        bool Save();
    }
}
