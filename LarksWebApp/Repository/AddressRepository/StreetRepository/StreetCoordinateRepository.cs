using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.AddressRepository.StreetRepository
{
    public class StreetCoordinateRepository : IStreetCoordinateRepository
    {
        private readonly LarksContext _dbContext;

        public StreetCoordinateRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DeleteStreetCoordinates(StreetCoordinates streetObj)
        {
             _dbContext.StreetCoordinates.Remove(streetObj);

            return Save();
        }

        public IList<StreetCoordinates> GetStreetCoordinates(Guid streetId)
        {
            return _dbContext.StreetCoordinates.Where(s => s.StreetId == streetId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public async Task<bool> StoreStreetCoordinates(IList<StreetCoordinates> streetCoordinates)
        {

            await _dbContext.StreetCoordinates.AddRangeAsync(streetCoordinates);

            return Save();
        }
    }
}
