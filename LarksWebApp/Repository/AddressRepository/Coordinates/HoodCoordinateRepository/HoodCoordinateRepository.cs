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

        public async Task<bool> CreateNeighbourhoodCoordinates(IList<SuburbCoordinates> listStreetCoordinates)
        {

            await _dbContext.SuburbCoordinates.AddRangeAsync(listStreetCoordinates);

            return Save();
        }

        public IList<SuburbCoordinates> GetNeighbourhoodCoordinates(Guid neighbourhoodCoordinatestreetId)
        {
           return _dbContext.SuburbCoordinates.Where(n => n.SuburbID == neighbourhoodCoordinatestreetId).ToList();    
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCoordiantes(SuburbCoordinates neighbourhood)
        {
            _dbContext.SuburbCoordinates.Update(neighbourhood);


            return Save();
        }
    }
}
