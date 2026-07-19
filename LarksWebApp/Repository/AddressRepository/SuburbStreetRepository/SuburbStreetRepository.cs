using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.SuburbStreetRepository
{
    public class SuburbStreetRepository : ISuburbStreetRepository
    {
        private readonly LarksContext _dbContext;
        public SuburbStreetRepository( LarksContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<SuburbStreets> AssociateStreetToSuburb(SuburbStreets suburbStreets)
        {
           await _dbContext.SuburbStreets.AddAsync(suburbStreets);


            if (Save())
            {
                return suburbStreets;
            }

            return null;
        }

        public async Task<bool> AssociateStreetToSuburbs(IList<SuburbStreets> suburbStreets)
        {

            await _dbContext.SuburbStreets.AddRangeAsync(suburbStreets);

            return Save();

        }

        public SuburbStreets GetSuburbStreet(Guid suburbId, Guid StreetId)
        {
            return _dbContext.SuburbStreets.Where(h => h.SuburbId == suburbId && h.StreetsID == StreetId)
                .Include(n =>n.Suburb)
                .Include(s =>s.Streets).FirstOrDefault();
        }

        public IList<SuburbStreets> GetStreetsInSuburb(Guid suburbId)
        {
            return _dbContext.SuburbStreets.Where(n => n.SuburbId == suburbId).Include(s => s.Streets).ToList();
        }

        public bool RemoveSuburbStreetAssociation(SuburbStreets suburbStreets)
        {
            _dbContext.SuburbStreets.Remove(suburbStreets);

            return Save();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }
    }
}
