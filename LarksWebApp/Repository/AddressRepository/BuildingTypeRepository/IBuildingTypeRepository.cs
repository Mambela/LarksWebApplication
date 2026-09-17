using LarksWebApp.Models;

namespace LarksWebApp.Repository.AddressRepository.BuildingTypeRepository
{
    public interface IBuildingTypeRepository
    {


        IList<BuildingTypeModel> GetAll(string lng);

        IList<BuildingTypeModel> GetAll();

        BuildingTypeModel GetBuildingType(Guid id);

        bool UpdateBuildingType(BuildingTypeModel buildingType);        


        Task<BuildingTypeModel> CreateBuildingType(BuildingTypeModel buildingType);

        bool Save();
    }
}
