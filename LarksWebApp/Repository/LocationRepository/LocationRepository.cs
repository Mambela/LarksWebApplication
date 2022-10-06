using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.LocationRepository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly LarksContext _dbContext;

        public LocationRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
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
            return _dbContext.Location.Where(s => s.CategoryId == CategoryId)
                .Include(s => s.Category).OrderBy(s=>s.Designation)
                .ToList();
        }

        public LocationModel GetLocation(int LocationId)
        {
            return _dbContext.Location.FirstOrDefault(c => c.Id == LocationId);
        }

        public IList<LocationModel> GetNeighbourhoodLocations(int neighbourhoodId)
        {
            return _dbContext.Location
                    .Include(n => n.Address).ThenInclude(n => n.Neighbourhood)
                        .Include(c =>c.Category)
                        .Include(s=>s.Subcategory)
                        .Include(l =>l.Images)
                        //.Include(a =>a.Address).ThenInclude(m=>m.Municipality)
                        .Include(a =>a.Address).ThenInclude(m=>m.Street)
                        //.Include(a =>a.Address).ThenInclude(m=>m.Neighbourhood)
                        //.Include(a =>a.Address).ThenInclude(m=>m.Province)
                        .Where(n => n.Address.NeighbourhoodId == neighbourhoodId) 
                        .ToList();
        }

        public IList<LocationModel> GetSubcategoryLocations(int SubcategoryId)
        {
            return _dbContext.Location.Where(s => s.SubcategoryId == SubcategoryId)
                .Include(s => s.Subcategory)
                .ToList();
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
    }
}
