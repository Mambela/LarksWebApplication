using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.StreetRepository
{
    public class StreetRepository : IStreetRepository
    {
        private readonly LarksContext _dbContext;

        public StreetRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<StreetModel> CreateStreetModel(StreetModel Street)
        {

            await _dbContext.Streets.AddAsync(Street);

            if (Save())
            {
                return Street;
            }

            return null;
        }

        public bool DeleteStreet(StreetModel street)
        {

            _dbContext.Streets.Remove(street);
            return Save();
        }

        public List<NeighbourhoodStreets> GetStreetInNeighBourhood(int neighbourhoodId)
        {
            return _dbContext.NeighbourhoodStreets.Where(n => n.NeighbourhoodID == neighbourhoodId).Include(s => s.Streets).ThenInclude(s =>s.StreetCoordinates).ToList();
        }

        public StreetModel GetStreetModel(int StreetId)
        {

            return _dbContext.Streets.Include(m => m.StreetCoordinates)
                .Include(s => s.NeighbourhoodStreets).ThenInclude(n => n.Neighbourhood)
                .FirstOrDefault(s => s.Id == StreetId);

        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateStreet(StreetModel street)
        {

            _dbContext.Streets.Update(street);

            return Save();
        }

        public bool VerifyStreetExist(string designation)
        {
            return _dbContext.Streets.Any(s => s.Designation.Trim().ToLower() == designation.Trim().ToLower());
        }

        public bool VerifyStreetExist(int Id)
        {
            return _dbContext.Streets.Any(s => s.Id == Id);
        }
    }
}
