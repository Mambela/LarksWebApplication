using LarksWebApp.Models;
using LarksWebApp.DAL;

namespace LarksWebApp.Repository.AddressRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LarksContext _dbContext;

        
        public AddressRepository(LarksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool AddressExists(string designation)
        {
           return  _dbContext.Addresses.Any(n => n.UnitReference.Trim() == designation.Trim());
        }

        public bool AddressExists(int AddressID)
        {
            return _dbContext.Addresses.Any(n => n.Id == AddressID);
        }


        public async Task<AddressModel> CreateAddress(AddressModel Address)
        {

            await _dbContext.Addresses.AddAsync(Address);

            var result = Save();

            if (result){

                return Address;
            }

            return null;
        }

        public bool DeleteAddress(AddressModel Address)
        {

            _dbContext.Addresses.Remove(Address);

                return Save();
        }

        public AddressModel GetAddress(int AddressId)
        {
            return _dbContext.Addresses.FirstOrDefault(a => a.Id == AddressId);
            
        }

        public List<AddressModel> GetAddressesInStreets(int streetsId)
        {
            return _dbContext.Addresses.Where(a => a.StreetID == streetsId).ToList();
        }

        public List<AddressModel> GetAddressesNearLocation(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0;
        }

        public bool UpdateAddress(AddressModel Address)
        {
            _dbContext.Addresses.Update(Address);

            return Save();
        }
    }
}
