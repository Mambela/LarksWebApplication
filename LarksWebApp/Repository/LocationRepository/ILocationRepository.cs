using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.LocationRepository
{
    public interface ILocationRepository
    {

        Task<LocationModel> CreateLocation(LocationModel Location);

        LocationModel GetLocation(int LocationId);

        IList<LocationModel> GetCategoryLocations(int CategoryId);

        IList<LocationModel> GetSubcategoryLocations(int SubcategoryId);

        IList<LocationModel> GetSuburbLocations(int suburbId);

        IList<LocationModel> LocationToInDistanceAsync(decimal lat,decimal lng,double distance,PaginationFilter pagination);
        IList<LocationModel> LocationCategoryByDistance(decimal lat,decimal lng,double distance,PaginationFilter pagination, int categoryId);
      
        int LocationCount(decimal lat,decimal lng,double distance, PaginationFilter pagination,string searchType, int CategoryId=0);

        int CountLocations(decimal lat, decimal lng, double distance,int categoryId = 0);

        Task<bool> Createlocationsubcategory(IEnumerable<LocationModelSubcategoryModel> Subcategory);

        Task<IList<LocationImageModel>> CreateLocationImage(IList<LocationImageModel> imageListLocation);

        Task<IList<LocationImageModel>> GetLocationImageModels(int locationId);

        bool RemoveLocationImage(LocationImageModel imageModel);


        bool Save();

    }
}
