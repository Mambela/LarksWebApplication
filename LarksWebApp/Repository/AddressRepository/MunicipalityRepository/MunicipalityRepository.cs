using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.MunicipalityRepository
{
    public class MunicipalityRepository : IMunicipalityRepository
    {
        private readonly LarksContext _dbContext;

        public MunicipalityRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<MunicipalityModel> CreateMunicipalityModel(MunicipalityModel municipality)
        {
            await _dbContext.Municipalities.AddAsync(municipality);

            if (Save())
            {
                return municipality;
            }

            return null;
        }

        public bool DeleteMunicipality(MunicipalityModel municipality)
        {
             _dbContext.Municipalities.Remove(municipality);

            return Save();
        }

        public MunicipalityModel GetMuncipality(int MunicipalityId)
        {

            return _dbContext.Municipalities.FirstOrDefault(m => m.Id == MunicipalityId);
        }

        public MunicipalityModel GetMuncipalityInCity(int provinceId, string MunicipalityDesigantion)
        {
            return _dbContext.Municipalities.FirstOrDefault(m => m.ProvinceId == provinceId && m.Designation.ToLower() == MunicipalityDesigantion );
        }

        public IList<MunicipalityModel> GetMunicipalityModels(int ProvinceId)
        {
            return _dbContext.Municipalities.Where(p => p.ProvinceId == ProvinceId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateMunicipality(MunicipalityModel municipality)
        {

            _dbContext.Municipalities.Update(municipality);
            return Save();
        }

        public MunicipalityModel VerifyMunicipalityExist(string designation,int provinceID)
        {
            return _dbContext.Municipalities.FirstOrDefault(m => m.Designation.Trim().ToLower() == designation.Trim().ToLower() && m.ProvinceId == provinceID);
        }

        public bool VerifyMunicipalityExist(int Id)
        {
            return _dbContext.Municipalities.Any(m => m.Id == Id);
        }
    }
}
