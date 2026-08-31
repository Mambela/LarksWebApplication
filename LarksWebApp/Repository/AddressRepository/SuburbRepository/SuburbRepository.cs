using LarksWebApp.DAL;
using LarksWebApp.DTOs;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.SuburbRepository
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

        public bool DeleteSuburb(SuburbModel suburb)
        {

            suburb.IsDeleted = true;
            _dbContext.Suburb.Update(suburb);

            return Save();
        }

        public SuburbModel GetSuburb(Guid suburbId)
        {
            return _dbContext.Suburb.Include(s =>s.StreetLinks)
                .ThenInclude(s =>s.Streets)
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(n =>n.Id == suburbId).FirstOrDefault();
        }

        public IList<SuburbModel> GetSuburbsInCities(Guid cityId)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(c => c.CityId == cityId).ToList();
        }

        public IList<SuburbModel> GetSuburbInDistricts(Guid districtiD)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(c => c.DistrictId == districtiD).ToList();
        }


        public IList<SuburbModel> GetSuburbsInMunicipality(Guid municipalityId)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(p =>p.MunicipalityId == municipalityId).ToList();
        }

        public IList<SuburbModel> GetSuburbs()
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .ToList();

        }

        public IList<SuburbModel> GetSuburbs(string sector, Guid id)
        {


            if (sector == "district")
            {
                return _dbContext.Suburb.Include(s => s.City).Include(s => s.District).Include(s => s.Municipality).ThenInclude(m => m.Province).Where(c => c.DistrictId == id).ToList();
            }
            else if (sector == "municipality")
            {
                return _dbContext.Suburb.Include(s => s.City).Include(s => s.District).Include(s => s.Municipality).ThenInclude(m => m.Province).Where(p => p.MunicipalityId == id).ToList();
            }
            else if (sector == "city")
            {
                return _dbContext.Suburb.Include(s => s.City).Include(s => s.District).Include(s => s.Municipality).ThenInclude(m => m.Province).Where(p => p.CityId == id).ToList();
            }

            return null;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateSuburb(SuburbModel suburb)
        {
            _dbContext.Suburb.Update(suburb);

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

        public bool VerifySuburbExistsById(Guid Id)
        {
            return _dbContext.Suburb.Any(n => n.Id == Id);
        }

        public SuburbModel GetSuburbInMunicipality(string designation, string municipalityId)
        {
            throw new NotImplementedException();
        }

        public IList<SuburbModel> GetSuburbs(Guid districtId, Guid municipalityId, Guid cityId)
        {

            return _dbContext.Suburb.Where(s => s.DistrictId == districtId || s.MunicipalityId == municipalityId || s.CityId == cityId)
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .ToList();
        }

        public IList<SuburbModel> GetSuburbs(IList<Guid> suburbs)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(c => suburbs.Contains(c.Id)).OrderBy(s =>s.Designation).ToList();
        }

        public IList<SuburbModel> GetSuburbsByName(string designation)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(s => s.Designation.Contains(designation)).ToList();
        }

        public IList<SuburbModel> GetSuburbsInProvince(Guid ProvinceId)
        {
            return _dbContext.Suburb
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Municipality).ThenInclude(m => m.Province)
                .Where(p =>p.Municipality.ProvinceId == ProvinceId).OrderBy(o =>o.Designation).ToList();
        }
    }
}
