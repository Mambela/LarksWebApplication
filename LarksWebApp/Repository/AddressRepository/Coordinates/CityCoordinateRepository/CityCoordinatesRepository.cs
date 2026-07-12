using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.CityCoordinateRepository
{
    public class CityCoordinatesRepository : ICityCoordinatesRepository
    {

        private readonly LarksContext _dbContext;

        public CityCoordinatesRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateCityCoordinates(IList<CityCoordinates> listCityCoordinates)
        {

            await _dbContext.CitiesCoordinates.AddRangeAsync(listCityCoordinates);

            return Save();
            
        }

        public IList<CityCoordinates> GetCityCoordinates(Guid cityId)
        {
           return _dbContext.CitiesCoordinates.Where(s => s.CityID == cityId).ToList();   
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateCoordiantes(CityCoordinates city)
        {
            _dbContext.CitiesCoordinates.Update(city);

            return Save();
        }
    }
}
