using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.LocationRepository
{
    public interface ILocationRepository
    {

        Task<LocationModel> CreateLocation(LocationModel Location);

        LocationModel GetLocation(int LocationId);

        //object GetLocationUsingStoreProc(string searchParam, string countryName, string categoryId = null, string suburbId = null
        //    , string cityId = null, string municipalityId = null,int pageSize=10,int skip = 0);

        IList<LocationModel> GetLocationUsingStoreProc( string countryName, string? searchParam, int? categoryId, int? suburbId, int? cityId, int? municipalityId, int pageSize = 10, int skip = 0);


        IList<LocationModel> GetCategoryLocations(int CategoryId, PaginationFilter filter);

        IList<LocationModel> GetSubcategoryLocations(int SubcategoryId);

        IList<LocationModel> GetSuburbLocations(int suburbId, PaginationFilter filter);

        IList<LocationModel> GetLocationsInCity(int CityId, PaginationFilter filter);

        IList<LocationModel> GetLocationsInMunicipality(int municipalityId, PaginationFilter filter);

        IList<LocationModel> LocationToInDistanceAsync(decimal lat, decimal lng, double distance, PaginationFilter pagination);
        IList<LocationModel> LocationCategoryByDistance(decimal lat, decimal lng, double distance, PaginationFilter pagination, int categoryId);
        public int CountLocationByAttribute(string attribute, int attributeId=0,string searchParam=null);


        int LocationCount(decimal lat, decimal lng, double distance, PaginationFilter pagination, string searchType, int CategoryId = 0);

        int CountLocations(decimal lat, decimal lng, double distance, int categoryId = 0);

        Task<bool> Createlocationsubcategory(IEnumerable<LocationModelSubcategoryModel> Subcategory);

        Task<IList<LocationImageModel>> CreateLocationImage(IList<LocationImageModel> imageListLocation);

        Task<IList<LocationImageModel>> GetLocationImageModels(int locationId);

        IList<LocationModel>GetLocationByLocationName(string searchParameter, PaginationFilter filter);


        IList<LocationModel> GetLocationList(IList<int> idList , PaginationFilter filter);
        

        bool RemoveLocationImage(LocationImageModel imageModel);


        bool Save();
        bool UpdateLocation(LocationModel location);
    }
}
