using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.AssoHoodStreetRepository
{
    public class NeighbourhoodStreeetRepository : INeighbourhoodStreet
    {
        private readonly LarksContext _dbContext;
        public NeighbourhoodStreeetRepository( LarksContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<SuburbStreets> AssociateStreetToNeighbourhood(SuburbStreets neighbourhood)
        {
           await _dbContext.SuburbStreets.AddAsync(neighbourhood);


            if (Save())
            {
                return neighbourhood;
            }

            return null;
        }

        public async Task<bool> AssociateStreetToNeighBourhoods(IList<SuburbStreets> neighbourhoodStreets)
        {

            await _dbContext.SuburbStreets.AddRangeAsync(neighbourhoodStreets);

            return Save();
            
        }

        public SuburbStreets GetNeighbourhoodStreet(int hoodId, int StreetId)
        {
            return _dbContext.SuburbStreets.Where(h => h.SuburbId == hoodId && h.StreetsID == StreetId)
                .Include(n =>n.Suburb)
                .Include(s =>s.Streets).FirstOrDefault();
        }

        public IList<SuburbStreets> GetStreetsInNeighbourhood(int hoodId)
        {
            return _dbContext.SuburbStreets.Where(n => n.SuburbId == hoodId).Include(s => s.Streets).ToList();
        }

        public bool RemoveNeighbourHoodStreetsAssociation(SuburbStreets neighbourhood)
        {
            _dbContext.SuburbStreets.Remove(neighbourhood);

            return Save();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }
    }
}
