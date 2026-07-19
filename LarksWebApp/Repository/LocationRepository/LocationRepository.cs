using AutoMapper;
using LarksWebApp.DAL;
using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace LarksWebApp.Repository.LocationRepository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly LarksContext _dbContext;
        private readonly IMapper mapper;

        public LocationRepository(LarksContext dbContext,IMapper _mapper)
        {
            _dbContext = dbContext;
            mapper = _mapper;
        }

        public async Task<LocationModel> CreateLocation(LocationModel Location)
        {
          await _dbContext.AddAsync(Location);

            if(Save())
                return Location;


            return null;
        }

        public IList<LocationModel> GetCategoryLocations(Guid CategoryId, PaginationFilter filter)
        {
            return _dbContext.Location
                .Where(s => s.CategoryId == CategoryId)
                .Include(s => s.Category)
                .Include(s => s.LocalSubcategories).ThenInclude(l =>l.Subcategories)
                .Include(l =>l.Images)
                .OrderBy(s=>s.Designation)
                .ToList();
        }

        public LocationModel GetLocation(Guid LocationId)
        {
            return _dbContext.Location
                .Include(c =>c.Category)
                .Include(c =>c.City)
                .Include(p=>p.Suburb.District)
                .Include(p=>p.Municipality.Province.Country)
                .Include(m =>m.Municipality)
                .Include(l =>l.LocalSubcategories).ThenInclude(c =>c.Subcategories)
                .Include(l =>l.Images)
                .Include(s =>s.Suburb)
                .FirstOrDefault(c => c.Id == LocationId);
        }

        public IList<LocationModel> GetSuburbLocations(Guid suburbId, PaginationFilter filter)
        {
            return _dbContext.Location
                        .Include(c =>c.Category)
                        .ThenInclude(c =>c.Subcategories)
                        .Include(l =>l.Images)
                        .Include(a =>a.Municipality)
                        .Include(a =>a.Suburb)
                        .Where(n => n.SuburbId == suburbId)
                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize)
                        .OrderBy(c =>c.Designation)
                        .ToList();
        }

        public IList<LocationModel> GetSubcategoryLocations(Guid SubcategoryId)
        {
            //return _dbContext.Location.Where(s => s.SubcategoryId == SubcategoryId)
            //    .Include(s => s.Subcategory)
            //    .ToList();

            throw new NotImplementedException();
        }

        public async Task<IList<LocationImageModel>> CreateLocationImage(IList<LocationImageModel> imageLocations)
        {
            await _dbContext.AddRangeAsync(imageLocations);

            if (Save())
                return imageLocations;

            return null;
        }

        public async Task<IList<LocationImageModel>> GetLocationImageModels(Guid locationId)
        {
            return await _dbContext.LocationImage.Where(l => l.LocationId == locationId).ToListAsync();
        }

        public bool RemoveLocationImage(LocationImageModel imageModel)
        {
            _dbContext.LocationImage.Remove(imageModel);
            return Save();

        }


        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public async Task<bool> Createlocationsubcategory(IEnumerable<LocationModelSubcategoryModel> LocationSubcategory)
        {
            await _dbContext.LocationModelSubcategoryModel.AddRangeAsync(LocationSubcategory);

            return Save();
        }

        public async Task<bool> RemoveLocationSubcategories(Guid locationId)
        {
            var existing = _dbContext.LocationModelSubcategoryModel.Where(l => l.LocationId == locationId);

            _dbContext.LocationModelSubcategoryModel.RemoveRange(existing);

            return Save();
        }

        public  IList<LocationModel> LocationToInDistanceAsync(decimal lat, decimal lng, double distance,PaginationFilter filter)
        {
            return  _dbContext.Location
                .FromSqlInterpolated($@"SELECT * FROM (
                    SELECT *,
                        (acos(
                            sin({lat} * pi() / 180) * sin(""Lat"" * pi() / 180)
                            + cos({lat} * pi() / 180) * cos(""Lat"" * pi() / 180) * cos(({lng} - ""Lng"") * pi() / 180)
                        ) * 180 / pi()) * 60 * 1.1515 * 1.609344 AS distance
                    FROM ""Location""
                ) AS loc
                WHERE distance <= {distance}")
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Include(l =>l.LocalSubcategories).ThenInclude(s =>s.Subcategories)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .OrderBy(c =>c.Designation)
                .ToList();
                       
        }

        public int LocationCount(decimal lat, decimal lng, double distance, PaginationFilter filter, string searchType,Guid? categoryId =null)
        {
            switch (searchType.ToUpper())
            {
                case ("SEARCHGEODISTANCE"):
                    return CountLocations(lat, lng, distance);
                case ("SEARCHGEOCATEGORY"):
                    return CountLocations(lat,lng,distance,categoryId);
                case ("SEARCHCATEGORYSUBCATEGORY"):
                    return 0;
                default:
                    return 0;
            }

        }

        public int CountLocations(decimal lat, decimal lng, double distance, Guid? categoryId = null)
        {
            var query = _dbContext.Location
                .FromSqlInterpolated($@"SELECT * FROM (
                    SELECT *,
                        (acos(
                            sin({lat} * pi() / 180) * sin(""Lat"" * pi() / 180)
                            + cos({lat} * pi() / 180) * cos(""Lat"" * pi() / 180) * cos(({lng} - ""Lng"") * pi() / 180)
                        ) * 180 / pi()) * 60 * 1.1515 * 1.609344 AS distance
                    FROM ""Location""
                ) AS loc
                WHERE distance <= {distance}")
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Include(l => l.LocalSubcategories).ThenInclude(s => s.Subcategories);

            if (categoryId != null)
            {
                return query.Where(c => c.CategoryId == categoryId).Count();
            }

            return query.Count();
        }

        

        public IList<LocationModel> LocationCategoryByDistance(decimal lat, decimal lng, double distance, PaginationFilter pagination, Guid categoryId)
        {
            return  _dbContext.Location
                .FromSqlInterpolated($@"SELECT * FROM (
                    SELECT *,
                        (acos(
                            sin({lat} * pi() / 180) * sin(""Lat"" * pi() / 180)
                            + cos({lat} * pi() / 180) * cos(""Lat"" * pi() / 180) * cos(({lng} - ""Lng"") * pi() / 180)
                        ) * 180 / pi()) * 60 * 1.1515 * 1.609344 AS distance
                    FROM ""Location""
                ) AS loc
                WHERE distance <= {distance}")
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Include(l => l.LocalSubcategories).ThenInclude(s => s.Subcategories)
                .Where(c  =>c.CategoryId == categoryId )
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .OrderBy(c =>c.Designation)
                .ToList();
        }

        public IList<LocationModel> GetLocationsInCity(Guid CityId, PaginationFilter filter)
        {

            //GetLocationsInCity

            return _dbContext.Location
                      .Include(c => c.Category)
                      .Include(l =>l.LocalSubcategories).ThenInclude(c => c.Subcategories)
                      .Include(l => l.Images)
                      .Include(a => a.Municipality)
                      .Include(a => a.Suburb)
                      .Include(c =>c.City)
                      .Where(n => n.CityId == CityId)
                      .Skip((filter.PageNumber - 1) * filter.PageSize)
                      .Take(filter.PageSize)
                      .OrderBy(c =>c.Designation)
                      .ToList();
        }

        public IList<LocationModel> GetLocationsInMunicipality(Guid municipalityId, PaginationFilter filter)
        {
            return _dbContext.Location
                       .Include(c => c.Category)
                       .Include(l =>l.LocalSubcategories).ThenInclude(c => c.Subcategories)
                       .Include(l => l.Images)
                       .Include(a => a.Municipality)
                       .Include(a => a.Suburb)
                       .Include(c => c.City)
                       .Include(m =>m.Municipality)
                       .Where(n => n.MunicipalityId == municipalityId)
                       .Skip((filter.PageNumber - 1) * filter.PageSize)
                       .Take(filter.PageSize)
                       .OrderBy(c =>c.Designation)
                       .ToList();
        }

        public int CountLocationByAttribute(string attribute, Guid? attributeId=null,string searchParam=null)
        {

            var filter = new PaginationFilter();
            switch (attribute.ToUpper().Trim())
            {
                case "CATEGORY":
                    return _dbContext.Location.Where(l =>l.CategoryId == attributeId).Count();
                case "SUBURB":
                    return _dbContext.Location.Where(l => l.SuburbId== attributeId).Count();

                case "CITY":
                    return _dbContext.Location.Where(l => l.CityId == attributeId).Count();

                case "MUNICIPALITY":
                    return _dbContext.Location.Where(l => l.MunicipalityId == attributeId).Count();
                case "LOCATION_NAME":
                    return _dbContext.Location.Where(l => l.Designation.Contains(searchParam)).Count();
                default:
                    return 0;
            }
        }

        public IList<LocationModel> GetLocationByLocationName(string searchParameter, PaginationFilter filter)
        {
            return _dbContext.Location
                        .Include(c => c.Category)
                       .Include(l => l.LocalSubcategories).ThenInclude(c => c.Subcategories)
                       .Include(l => l.Images)
                       .Include(a => a.Municipality)
                       .Include(a => a.Suburb)
                       .Include(c => c.City)
                       .Include(m => m.Municipality)
                       .Where(n => n.Designation.Contains(searchParameter))
                       .Skip((filter.PageNumber - 1) * filter.PageSize)
                       .Take(filter.PageSize)
                       .OrderBy(c =>c.Designation)
                       .ToList(); ;
        }

        public bool UpdateLocation(LocationModel location)
        {
             _dbContext.Update(location);
            return Save();
        }

 

        public IList<LocationModel> GetLocationUsingStoreProc(string countryName, string? searchParam, Guid? categoryId, Guid? suburbId, Guid? cityId, Guid? municipalityId, int pageSize = 10, int skip = 0)
        {
            return _dbContext.Location
                .Include(c => c.City)
                .Include(c => c.Category)
                .Include(l => l.LocalSubcategories).ThenInclude(c => c.Subcategories)
                .Include(m => m.Municipality)
                .Include(s => s.Suburb)
                .Include(l => l.Images)
                .Include(c => c.Municipality).ThenInclude(p => p.Province).ThenInclude(c => c.Country)
                .Where(l => ((l.Designation.Contains(searchParam) || searchParam == null || searchParam.Equals("")) && l.Municipality.Province.Country.Designation.Contains(countryName))
                    && ((categoryId == null) || (l.CategoryId == categoryId))
                    && ((suburbId == null) || (l.SuburbId == suburbId))
                    && ((municipalityId == null) || (l.MunicipalityId == municipalityId)))
                .Take(pageSize)
                .Skip(skip)
                .OrderBy(c =>c.Designation)
                .ToList();
        }

        public IList<LocationModel> GetLocationList(IList<Guid> idList, PaginationFilter filter)
        {
            return _dbContext.Location
                .Include(c => c.Category)
                .Include(l => l.LocalSubcategories).ThenInclude(c => c.Subcategories)
                .Include(m => m.City)
                .Include(m => m.Municipality).ThenInclude(p =>p.Province).ThenInclude(c => c.Country)
                .Include(s => s.Suburb)
                .Include(m =>m.Images)
                .Where(l => idList.Contains(l.Id))
                .Take(filter.PageSize)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .OrderBy(c =>c.Designation)
                .ToList();
        }
    }
}
