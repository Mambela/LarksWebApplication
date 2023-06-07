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

        public IList<LocationModel> GetCategoryLocations(int CategoryId)
        {
            var locationObjectList =  _dbContext.Location.Where(s => s.CategoryId == CategoryId)
                .Include(s => s.Category)
                .Include(s => s.LocalSubcategories).ThenInclude(l =>l.Subcategories)
                .Include(l =>l.Images)
                .OrderBy(s=>s.Designation)
                .ToList();

            var categoryLocations = mapper.Map<IList<LocationModel>>(locationObjectList);


            return categoryLocations;
        }

        public LocationModel GetLocation(int LocationId)
        {
            return _dbContext.Location.FirstOrDefault(c => c.Id == LocationId);
        }

        public IList<LocationModel> GetSuburbLocations(int suburbId)
        {
            return _dbContext.Location
                        .Include(c =>c.Category)
                        .ThenInclude(c =>c.Subcategories)
                        .Include(l =>l.Images)
                        .Include(a =>a.Municipality)
                        .Include(a =>a.Suburb)
                        .Where(n => n.SuburbId == suburbId) 
                        .ToList();
        }

        public IList<LocationModel> GetSubcategoryLocations(int SubcategoryId)
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

        public async Task<IList<LocationImageModel>> GetLocationImageModels(int locationId)
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

        public  IList<LocationModel> LocationToInDistanceAsync(decimal lat, decimal lng, double distance,PaginationFilter filter)
        {

            string sqlLocation = string.Format("SELECT * FROM (" +
                "SELECT *, " +
                "(((" +
                " acos(" +
                " sin(( {0} * pi() / 180)) * sin(( `Lat` * pi() / 180)) + cos(( {0} * pi() /180 )) *" +
                " cos(( `Lat` * pi() / 180)) * cos((( {1} - `Lng`) * pi()/180)))" +
                " ) * 180/pi()) * 60 * 1.1515 * 1.609344) " +
                "as distance FROM `Location` ) Location " +
                "WHERE distance <= {2} ",lat,lng,distance);


            return  _dbContext.Location
                .FromSqlRaw(sqlLocation)
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Include(l =>l.LocalSubcategories).ThenInclude(s =>s.Subcategories)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
                       
        }

        public int LocationCount(decimal lat, decimal lng, double distance, PaginationFilter filter, string searchType,int categoryId =0)
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

        public int CountLocations(decimal lat, decimal lng, double distance, int categoryId = 0)
        {
            string sqlLocation = string.Format("SELECT * FROM (" +
                "SELECT *, " +
                "(((" +
                " acos(" +
                " sin(( {0} * pi() / 180)) * sin(( `Lat` * pi() / 180)) + cos(( {0} * pi() /180 )) *" +
                " cos(( `Lat` * pi() / 180)) * cos((( {1} - `Lng`) * pi()/180)))" +
                " ) * 180/pi()) * 60 * 1.1515 * 1.609344) " +
                "as distance FROM `Location` ) Location " +
                "WHERE distance <= {2} ", lat, lng, distance);


            if (categoryId != 0)
            {

                return _dbContext.Location
                .FromSqlRaw(sqlLocation)
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Where(c =>c.CategoryId == categoryId)
                .Include(l => l.LocalSubcategories).ThenInclude(s => s.Subcategories).Count();
            }

            return _dbContext.Location
            .FromSqlRaw(sqlLocation)
            .Include(c => c.Category)
            .Include(l => l.Images)
            .Include(c => c.City)
            .Include(s => s.Suburb)
            .Include(m => m.Municipality)
            .Include(l => l.LocalSubcategories).ThenInclude(s => s.Subcategories).Count();

        }

        public IList<LocationModel> LocationCategoryByDistance(decimal lat, decimal lng, double distance, PaginationFilter pagination, int categoryId)
        {
            string sqlLocation = string.Format("SELECT * FROM (" +
              "SELECT *, " +
              "(((" +
              " acos(" +
              " sin(( {0} * pi() / 180)) * sin(( `Lat` * pi() / 180)) + cos(( {0} * pi() /180 )) *" +
              " cos(( `Lat` * pi() / 180)) * cos((( {1} - `Lng`) * pi()/180)))" +
              " ) * 180/pi()) * 60 * 1.1515 * 1.609344) " +
              "as distance FROM `Location` ) Location " +
              "WHERE distance <= {2} ", lat, lng, distance);


            return  _dbContext.Location
                .FromSqlRaw(sqlLocation)
                .Include(c => c.Category)
                .Include(l => l.Images)
                .Include(c => c.City)
                .Include(s => s.Suburb)
                .Include(m => m.Municipality)
                .Include(l => l.LocalSubcategories).ThenInclude(s => s.Subcategories)
                .Where(c  =>c.CategoryId == categoryId )
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
        }
    }
}
