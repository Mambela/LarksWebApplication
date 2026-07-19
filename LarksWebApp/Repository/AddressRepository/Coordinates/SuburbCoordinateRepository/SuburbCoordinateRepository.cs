using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.SuburbCoordinateRepository
{
    public class SuburbCoordinateRepository : ISuburbCoordinateRepository
    {
        private readonly LarksContext _dbContext;

        public SuburbCoordinateRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateSuburbCoordinates(IList<SuburbCoordinates> listStreetCoordinates)
        {

            await _dbContext.SuburbCoordinates.AddRangeAsync(listStreetCoordinates);

            return Save();
        }

        public IList<SuburbCoordinates> GetSuburbCoordinates(Guid suburbId)
        {
           return _dbContext.SuburbCoordinates.Where(n => n.SuburbID == suburbId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCoordiantes(SuburbCoordinates suburb)
        {
            _dbContext.SuburbCoordinates.Update(suburb);


            return Save();
        }
    }
}
