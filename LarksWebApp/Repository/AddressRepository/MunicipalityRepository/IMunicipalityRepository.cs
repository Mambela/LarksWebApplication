using LarksWebApp.Models;

namespace LarksWebApp.Repository.MunicipalityRepository
{
    public interface IMunicipalityRepository
    {

        MunicipalityModel VerifyMunicipalityExist(string designation,int provinceId);

        bool VerifyMunicipalityExist(int Id);

        Task<MunicipalityModel> CreateMunicipalityModel(MunicipalityModel municipality);

        MunicipalityModel GetMuncipality(int MunicipalityId);
        MunicipalityModel GetMuncipalityInCity(int cityId,string MunicipalityDesigantion);

        IList<MunicipalityModel> GetMunicipalityModels(int ProvinceId);

        public bool DeleteMunicipality(MunicipalityModel municipality);


        public bool UpdateMunicipality(MunicipalityModel municipality);

        bool Save();
    }
}
