using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.HoodCoordinateRepository
{
    public class HoodCoordinateRepository : IHoodCoordinateRepository
    {
        private readonly LarksContext _dbContext;

        public HoodCoordinateRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateNeighbourhoodCoordinates(IList<NeighbourhoodCoordinates> listStreetCoordinates)
        {

            await _dbContext.NeighbourhoodCoordinates.AddRangeAsync(listStreetCoordinates);

            return Save();
        }

        public IList<NeighbourhoodCoordinates> GetNeighbourhoodCoordinates(int neighbourhoodCoordinatestreetId)
        {
           return _dbContext.NeighbourhoodCoordinates.Where(n => n.NeighbourhoodID == neighbourhoodCoordinatestreetId).ToList();    
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCoordiantes(NeighbourhoodCoordinates neighbourhood)
        {
            _dbContext.NeighbourhoodCoordinates.Update(neighbourhood);


            return Save();
        }
    }
}
