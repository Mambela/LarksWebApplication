using LarksWebApp.Models;

namespace LarksWebApp.Repository.LocationRepository
{
    public interface ILocationRepository
    {

        Task<LocationModel> CreateLocation(LocationModel Location);

        LocationModel GetLocation(int LocationId);

        IList<LocationModel> GetCategoryLocations(int CategoryId);

        IList<LocationModel> GetSubcategoryLocations(int SubcategoryId);

        IList<LocationModel> GetNeighbourhoodLocations(int LocationId);

        Task<IList<LocationImageModel>> CreateLocationImage(IList<LocationImageModel> imageListLocation);

        Task<IList<LocationImageModel>> GetLocationImageModels(int locationId);

        bool RemoveLocationImage(LocationImageModel imageModel);


        bool Save();

    }
}
