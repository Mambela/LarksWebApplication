using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.AddressRepository.BuildingTypeRepository
{
    public class BuildingTypeRepository : IBuildingTypeRepository
    {
        private readonly LarksContext _dbContext;

        public BuildingTypeRepository(LarksContext larksContext)
        {
            _dbContext = larksContext;
        }

        public async Task<BuildingTypeModel> CreateBuildingType(BuildingTypeModel buildingType)
        {
            await _dbContext.BuildingType.AddAsync(buildingType);

            if (Save())
            {
                return buildingType ;
            }

            return null;

        }

        public IList<BuildingTypeModel> GetAll(string lng)
        {
            return _dbContext.BuildingType.Where(l =>l.Lang == lng).ToList();
        }

        public BuildingTypeModel GetBuildingType(Guid id)
        {
            return _dbContext.BuildingType.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateBuildingType(BuildingTypeModel buildingType)
        {
            _dbContext.Update(buildingType);

            return Save();
        }
    }
}
