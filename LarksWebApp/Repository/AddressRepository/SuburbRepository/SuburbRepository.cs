using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.HoodRepository
{
    public class SuburbRepository : ISuburbRepository
    {
        private readonly LarksContext _dbContext;

        public SuburbRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SuburbModel> CreateSuburb(SuburbModel suburb)
        {
            await _dbContext.Suburb.AddAsync(suburb);

            if (Save())
            {
                return suburb;
            }

            return null;
        }

        public bool DeleteNeighbourhood(SuburbModel suburb)
        {

            _dbContext.Suburb.Remove(suburb);

            return Save();
        }

        public SuburbModel GetNeighbourhood(int suburbId)
        {
            return _dbContext.Suburb.Include(s =>s.NeighbourhoodStreets)
                .ThenInclude(s =>s.Streets).Where(n =>n.Id == suburbId).FirstOrDefault();
        }

        public IList<SuburbModel> GetNeighbourhoodInCities(int cityId)
        {
            return _dbContext.Suburb.Where(c => c.CityId == cityId).ToList();
        }

        public IList<SuburbModel> GetNeighbourhoodInDistricts(int districtiD)
        {
            return _dbContext.Suburb.Where(c => c.DistrictId == districtiD).ToList();
        }
 

        public IList<SuburbModel> GetNeighbourhoodInMunicipality(int municipalityId)
        {
            return _dbContext.Suburb.Where(p =>p.MunicipalityId == municipalityId).ToList();
        }

        public IList<SuburbModel> GetNeighbourhoodModels()
        {
            return _dbContext.Suburb.ToList();

        }

        public IList<SuburbModel> GetNeighbourhoodModels(string sector, int id)
        {


            if (sector == "district")
            {
                return _dbContext.Suburb.Where(c => c.DistrictId == id).ToList();
            }
            else if (sector == "municipality")
            {
                return _dbContext.Suburb.Where(p => p.MunicipalityId == id).ToList();
            }
            else if (sector == "city")
            {
                return _dbContext.Suburb.Where(p => p.CityId == id).ToList();
            }

            return null;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateNeighbourhoody(SuburbModel neighbourhood)
        {
            _dbContext.Suburb.Update(neighbourhood);

            return Save();
        }

        public SuburbModel VerifySuburbExist(string designation, int municipalityId, int cityId, int districtId)
        {
            var suburb =_dbContext.Suburb.FirstOrDefault(n =>n.Designation.Trim() == designation.Trim() && n.MunicipalityId == municipalityId);

            if (suburb != null)
                return suburb;

            suburb = _dbContext.Suburb.FirstOrDefault(n => n.Designation.Trim() == designation.Trim() && n.DistrictId == districtId);

            if (suburb != null)
                return suburb;

           return _dbContext.Suburb.FirstOrDefault(n => n.Designation.Trim() == designation.Trim() && n.CityId == cityId);          
        }

        public bool VerifyNeighbourhoodExist(int Id)
        {
            return _dbContext.Suburb.Any(n => n.Id == Id);
        }

        public SuburbModel GetSuburbInMunicipality(string designation, string municipalityId)
        {
            throw new NotImplementedException();
        }

        public IList<SuburbModel> GetNeighbourhoodModels(int districtId, int municipalityId, int cityId)
        {

            return _dbContext.Suburb.Where(s => s.DistrictId == districtId || s.MunicipalityId == municipalityId || s.CityId == cityId).ToList();
        }

        public IList<SuburbModel> GetSuburbs(IList<int> suburbs)
        {
            return _dbContext.Suburb.Where(c => suburbs.Contains(c.Id)).OrderBy(s =>s.Designation).ToList();
        }
    }
}
