using LarksWebApp.Models;

namespace LarksWebApp.Repository.MunicipalityRepository
{
    public interface IMunicipalityRepository
    {

        MunicipalityModel VerifyMunicipalityExist(string designation,Guid provinceId);

        bool VerifyMunicipalityExist(Guid Id);

        Task<MunicipalityModel> CreateMunicipalityModel(MunicipalityModel municipality);

        MunicipalityModel GetMuncipality(Guid MunicipalityId);
        MunicipalityModel GetMuncipalityInCity(Guid cityId,string MunicipalityDesigantion);

        IList<MunicipalityModel> GetMunicipalityModels(Guid ProvinceId);

        IList<MunicipalityModel> GetAllMunicipalities();

        public bool DeleteMunicipality(MunicipalityModel municipality);


        public bool UpdateMunicipality(MunicipalityModel municipality);

        bool Save();
    }
}
