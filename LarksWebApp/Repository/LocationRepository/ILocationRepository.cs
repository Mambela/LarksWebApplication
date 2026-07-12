using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.LocationRepository
{
    public interface ILocationRepository
    {

        Task<LocationModel> CreateLocation(LocationModel Location);

        LocationModel GetLocation(Guid LocationId);

        //object GetLocationUsingStoreProc(string searchParam, string countryName, string categoryId = null, string suburbId = null
        //    , string cityId = null, string municipalityId = null,int pageSize=10,int skip = 0);

        IList<LocationModel> GetLocationUsingStoreProc( string countryName, string? searchParam, Guid? categoryId, Guid? suburbId, Guid? cityId, Guid? municipalityId, int pageSize = 10, int skip = 0);


        IList<LocationModel> GetCategoryLocations(Guid CategoryId, PaginationFilter filter);

        IList<LocationModel> GetSubcategoryLocations(Guid SubcategoryId);

        IList<LocationModel> GetSuburbLocations(Guid suburbId, PaginationFilter filter);

        IList<LocationModel> GetLocationsInCity(Guid CityId, PaginationFilter filter);

        IList<LocationModel> GetLocationsInMunicipality(Guid municipalityId, PaginationFilter filter);

        IList<LocationModel> LocationToInDistanceAsync(decimal lat, decimal lng, double distance, PaginationFilter pagination);
        IList<LocationModel> LocationCategoryByDistance(decimal lat, decimal lng, double distance, PaginationFilter pagination, Guid categoryId);
        public int CountLocationByAttribute(string attribute, Guid? attributeId=null,string searchParam=null);


        int LocationCount(decimal lat, decimal lng, double distance, PaginationFilter pagination, string searchType, Guid? CategoryId = null);

        int CountLocations(decimal lat, decimal lng, double distance, Guid? categoryId = null);

        Task<bool> Createlocationsubcategory(IEnumerable<LocationModelSubcategoryModel> Subcategory);

        Task<IList<LocationImageModel>> CreateLocationImage(IList<LocationImageModel> imageListLocation);

        Task<IList<LocationImageModel>> GetLocationImageModels(Guid locationId);

        IList<LocationModel>GetLocationByLocationName(string searchParameter, PaginationFilter filter);


        IList<LocationModel> GetLocationList(IList<Guid> idList , PaginationFilter filter);
        

        bool RemoveLocationImage(LocationImageModel imageModel);


        bool Save();
        bool UpdateLocation(LocationModel location);
    }
}
