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
        public async Task<NeighbourhoodStreets> AssociateStreetToNeighbourhood(NeighbourhoodStreets neighbourhood)
        {
           await _dbContext.NeighbourhoodStreets.AddAsync(neighbourhood);


            if (Save())
            {
                return neighbourhood;
            }

            return null;
        }

        public async Task<bool> AssociateStreetToNeighBourhoods(IList<NeighbourhoodStreets> neighbourhoodStreets)
        {

            await _dbContext.NeighbourhoodStreets.AddRangeAsync(neighbourhoodStreets);

            return Save();
            
        }

        public NeighbourhoodStreets GetNeighbourhoodStreet(int hoodId, int StreetId)
        {
            return _dbContext.NeighbourhoodStreets.Where(h => h.NeighbourhoodID == hoodId && h.StreetsID == StreetId)
                .Include(n =>n.Neighbourhood)
                .Include(s =>s.Streets).FirstOrDefault();
        }

        public IList<NeighbourhoodStreets> GetStreetsInNeighbourhood(int hoodId)
        {
            return _dbContext.NeighbourhoodStreets.Where(n => n.NeighbourhoodID == hoodId).Include(s => s.Streets).ToList();
        }

        public bool RemoveNeighbourHoodStreetsAssociation(NeighbourhoodStreets neighbourhood)
        {
            _dbContext.NeighbourhoodStreets.Remove(neighbourhood);

            return Save();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }
    }
}
