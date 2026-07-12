using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.StreetCoordinateRepository
{
    public class StreetCoordinatesRepository : IStreetCoordinatesRepository
    {

        private readonly LarksContext _dbContext;

        public StreetCoordinatesRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
     
        public async Task<bool> CreateStreetCoordinates(IList<StreetCoordinates> streetCoordinates)
        {
            await _dbContext.StreetCoordinates.AddRangeAsync(streetCoordinates);

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

        public bool UpdateCoordiantes(StreetCoordinates street)
        {
           _dbContext.StreetCoordinates.Update(street);

            return Save();
        }
    }
}
