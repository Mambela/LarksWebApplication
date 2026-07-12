using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.ProviceCoordinateRepository
{
    public class ProvinceCoordinatesRepository : IProvinceCoordinatesRepository
    {

        private readonly LarksContext _dbContext;

        public ProvinceCoordinatesRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
    

        public async Task<bool> CreateProvinceCoordinates(IList<ProvinceCoordinates> listStreetCoordinates)
        {

            await _dbContext.ProvinceCoordinates.AddRangeAsync(listStreetCoordinates);

            return Save();
        }

        public IList<ProvinceCoordinates> GetProvinceCoordinates(Guid ProvinceId)
        {
            return _dbContext.ProvinceCoordinates.Where(p => p.ProvinceId == ProvinceId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCoordiantes(ProvinceCoordinates province)
        {
            _dbContext.ProvinceCoordinates.Update(province);
            return Save();
        }
    }
}
