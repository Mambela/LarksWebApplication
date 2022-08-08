using LarksWebApp.DAL;
using LarksWebApp.Models;

namespace LarksWebApp.Repository.HoodRepository
{
    public class NeighbourhoodRepository : INeighbourhoodRepository
    {
        private readonly LarksContext _dbContext;

        public NeighbourhoodRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NeighbourhoodModel> CreateNeighbourhood(NeighbourhoodModel neighbourhood)
        {
            await _dbContext.Neighbourhoods.AddAsync(neighbourhood);

            if (Save())
            {
                return neighbourhood;
            }

            return null;
        }

        public bool DeleteNeighbourhood(NeighbourhoodModel neighbourhood)
        {

            _dbContext.Neighbourhoods.Remove(neighbourhood);

            return Save();
        }

        public NeighbourhoodModel GetNeighbourhood(int neighbourhoodId)
        {
            return _dbContext.Neighbourhoods.FirstOrDefault(n => n.Id == neighbourhoodId);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateNeighbourhoody(NeighbourhoodModel neighbourhood)
        {
            _dbContext.Neighbourhoods.Update(neighbourhood);

            return Save();
        }

        public bool VerifyNeighbourhoodExist(string designation, int muncipalityId)
        {
            return _dbContext.Neighbourhoods.Any(n =>n.Designation.Trim() == designation.Trim() && n.MunicipalityId == muncipalityId);
        }

        public bool VerifyNeighbourhoodExist(int Id)
        {
            return _dbContext.Neighbourhoods.Any(n => n.Id == Id);
        }
    }
}
