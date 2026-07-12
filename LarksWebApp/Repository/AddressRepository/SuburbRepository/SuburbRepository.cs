using LarksWebApp.DAL;
using LarksWebApp.DTOs;
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

        public SuburbModel GetSuburb(Guid suburbId)
        {
            return _dbContext.Suburb.Include(s =>s.NeighbourhoodStreets)
                .ThenInclude(s =>s.Streets).Where(n =>n.Id == suburbId).FirstOrDefault();
        }

        public IList<SuburbModel> GetSuburbsInCities(Guid cityId)
        {
            return _dbContext.Suburb.Where(c => c.CityId == cityId).ToList();
        }

        public IList<SuburbModel> GetSuburbInDistricts(Guid districtiD)
        {
            return _dbContext.Suburb.Where(c => c.DistrictId == districtiD).ToList();
        }


        public IList<SuburbModel> GetNeighbourhoodInMunicipality(Guid municipalityId)
        {
            return _dbContext.Suburb.Where(p =>p.MunicipalityId == municipalityId).ToList();
        }

        public IList<SuburbModel> GetNeighbourhoodModels()
        {
            return _dbContext.Suburb.ToList();

        }

        public IList<SuburbModel> GetNeighbourhoodModels(string sector, Guid id)
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

        public SuburbModel VerifySuburbExist(string designation, Guid? municipalityId, Guid? cityId, Guid? districtId)
        {
            var suburb =_dbContext.Suburb.FirstOrDefault(n =>n.Designation.Trim() == designation.Trim() && n.MunicipalityId == municipalityId);

            if (suburb != null)
                return suburb;

            suburb = _dbContext.Suburb.FirstOrDefault(n => n.Designation.Trim() == designation.Trim() && n.DistrictId == districtId);

            if (suburb != null)
                return suburb;

           return _dbContext.Suburb.FirstOrDefault(n => n.Designation.Trim() == designation.Trim() && n.CityId == cityId);          
        }

        public bool VerifyNeighbourhoodExist(Guid Id)
        {
            return _dbContext.Suburb.Any(n => n.Id == Id);
        }

        public SuburbModel GetSuburbInMunicipality(string designation, string municipalityId)
        {
            throw new NotImplementedException();
        }

        public IList<SuburbModel> GetNeighbourhoodModels(Guid districtId, Guid municipalityId, Guid cityId)
        {

            return _dbContext.Suburb.Where(s => s.DistrictId == districtId || s.MunicipalityId == municipalityId || s.CityId == cityId).ToList();
        }

        public IList<SuburbModel> GetSuburbs(IList<Guid> suburbs)
        {
            return _dbContext.Suburb.Where(c => suburbs.Contains(c.Id)).OrderBy(s =>s.Designation).ToList();
        }

        public IList<SuburbModel> GetSuburbsByName(string designation)
        {
            return _dbContext.Suburb.Where(s => s.Designation.Contains(designation)).ToList();
        }

        public IList<SuburbModel> GetSuburbsInProvince(Guid ProvinceId)
        {
            return _dbContext.Suburb.Where(p =>p.Municipality.ProvinceId == ProvinceId).OrderBy(o =>o.Designation).ToList();   
        }
    }
}
