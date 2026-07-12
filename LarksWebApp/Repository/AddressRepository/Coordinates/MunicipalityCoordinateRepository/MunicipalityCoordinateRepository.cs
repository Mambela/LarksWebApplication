using LarksWebApp.DAL;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Repository.Coordinates.MunicipalityCoordinateRepository
{
    public class MunicipalityCoordinateRepository : IMunicipalityCoordinateRepository
    {

        private readonly LarksContext _dbContext;
        public MunicipalityCoordinateRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateMunicipalityCoordinates(IList<MunicipalityCoordinates> listMunicipalityCoordinates)
        {
            await _dbContext.MunicipalityCoordinates.AddRangeAsync(listMunicipalityCoordinates);

            return Save();
        }

        public IList<MunicipalityCoordinates> GetMunicipalityCoordinates(Guid municipalityId)
        {
            return _dbContext.MunicipalityCoordinates.Where(M => M.MunicipalityID == municipalityId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0; 
        }

        public bool UpdateCoordiantes(MunicipalityCoordinates municipality)
        {

            _dbContext.MunicipalityCoordinates.Update(municipality);
            return Save();
        }
    }
}
