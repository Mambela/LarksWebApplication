using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.HoodRepository
{
    public class NeighbourhoodRepository : INeighbourhoodRepository
    {
        private readonly LarksContext _dbContext;

        public NeighbourhoodRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NeighbourhoodModel> CreateNeighbourhood(NeighbourhoodModel neighbourhood)
        {
            await _dbContext.Neighbourhoods.AddAsync(neighbourhood);

            if (Save())
            {
                return neighbourhood;
            }

            return null;
        }

        public bool DeleteNeighbourhood(NeighbourhoodModel neighbourhood)
        {

            _dbContext.Neighbourhoods.Remove(neighbourhood);

            return Save();
        }

        public NeighbourhoodModel GetNeighbourhood(int neighbourhoodId)
        {
            return _dbContext.Neighbourhoods.Include(s =>s.NeighbourhoodStreets)
                .ThenInclude(s =>s.Streets).Where(n =>n.Id == neighbourhoodId).FirstOrDefault();
        }

        public IList<NeighbourhoodModel> GetNeighbourhoodInCities(int cityId)
        {
            return _dbContext.Neighbourhoods.Where(c => c.CityId == cityId).ToList();
        }

        public IList<NeighbourhoodModel> GetNeighbourhoodInDistricts(int districtiD)
        {
            return _dbContext.Neighbourhoods.Where(c => c.DistrictId == districtiD).ToList();
        }
 

        public IList<NeighbourhoodModel> GetNeighbourhoodInMunicipality(int municipalityId)
        {
            return _dbContext.Neighbourhoods.Where(p =>p.MunicipalityId == municipalityId).ToList();
        }

        public IList<NeighbourhoodModel> GetNeighbourhoodModels()
        {
            return _dbContext.Neighbourhoods.ToList();

        }

        public IList<NeighbourhoodModel> GetNeighbourhoodModels(string sector, int id)
        {


            if (sector == "districts")
            {
                return _dbContext.Neighbourhoods.Where(c => c.DistrictId == id).ToList();
            }
            else if (sector == "municipality")
            {
                return _dbContext.Neighbourhoods.Where(p => p.MunicipalityId == id).ToList();
            }
            else if (sector == "city")
            {
                return _dbContext.Neighbourhoods.Where(p => p.CityId == id).ToList();
            }

            return null;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateNeighbourhoody(NeighbourhoodModel neighbourhood)
        {
            _dbContext.Neighbourhoods.Update(neighbourhood);

            return Save();
        }

        public bool VerifyNeighbourhoodExist(string designation, int muncipalityId)
        {
            return _dbContext.Neighbourhoods.Any(n =>n.Designation.Trim() == designation.Trim() && n.MunicipalityId == muncipalityId);
        }

        public bool VerifyNeighbourhoodExist(int Id)
        {
            return _dbContext.Neighbourhoods.Any(n => n.Id == Id);
        }
    }
}
