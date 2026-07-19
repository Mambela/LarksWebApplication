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

        public async Task<bool> CreateSuburbStreetsLink(SuburbStreets suburbStreets)
        {

            await _dbContext.SuburbStreets.AddAsync(suburbStreets);
            return Save();
        }

        public bool DeleteStreet(StreetModel street)
        {

            street.IsDeleted = true;
            _dbContext.Streets.Update(street);
            return Save();
        }

        public List<SuburbStreets> GetStreetsInSuburb(Guid suburbId)
        {
            return _dbContext.SuburbStreets.Where(n => n.SuburbId == suburbId).Include(s => s.Streets).ThenInclude(s =>s.StreetCoordinates).ToList();
        }

        public List<SuburbStreets> GetStreetsInSuburbByValue(Guid suburbId, string streetValue)
        {

            return _dbContext.SuburbStreets.Where(n => n.Streets.Designation.Contains(streetValue) && n.SuburbId == suburbId)
                .Include(s => s.Streets).ThenInclude(s => s.StreetCoordinates).ToList();
            throw new NotImplementedException();
        }

        public StreetModel GetStreetModel(Guid StreetId)
        {

            return _dbContext.Streets.Include(m => m.StreetCoordinates)
                .Include(s => s.SuburbLinks).ThenInclude(n => n.Suburb)
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

        public bool VerifyStreetExist(Guid Id)
        {
            return _dbContext.Streets.Any(s => s.Id == Id);
        }

        public StreetModel VerifyStreetExistance(string designation)
        {
            return _dbContext.Streets.FirstOrDefault(s => s.Designation.ToLower() == designation.ToLower());
        }

        public bool VerifyStreetSuburbLink(Guid streetsId, Guid suburbId)
        {
            return _dbContext.SuburbStreets.Any(s => s.StreetsID == streetsId && s.SuburbId == suburbId);
        }
    }
}
