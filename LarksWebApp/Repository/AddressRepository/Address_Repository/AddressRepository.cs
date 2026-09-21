using LarksWebApp.DAL;
using LarksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.Repository.AddressRepository.Address_Repository
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
            return _dbContext.Addresses.Any(n => n.StreetNumber.Trim() == designation.Trim());
        }

        public bool AddressExists(Guid AddressID)
        {
            return _dbContext.Addresses.Any(n => n.Id == AddressID);
        }


        public async Task<AddressModel> CreateAddress(AddressModel Address)
        {

            await _dbContext.Addresses.AddAsync(Address);

            var result = Save();

            if (result)
            {

                return Address;
            }

            return null;
        }

        public bool DeleteAddress(AddressModel Address)
        {

            _dbContext.Addresses.Remove(Address);

            return Save();
        }

        public AddressModel GetAddress(Guid AddressId)
        {
            return _dbContext.Addresses
                .Include(n => n.Suburb)
                .Include(m => m.Municipality)
                .Include(s => s.Street)
                .Include(p => p.Province)
                .Include(c => c.Country)
                .FirstOrDefault(a => a.Id == AddressId);

        }

        public List<AddressModel> GetAddressesInStreets(Guid streetsId)
        {
            return _dbContext.Addresses.Where(a => a.StreetID == streetsId)
                .Include(n =>n.Suburb)
                .Include(m => m.Municipality)
                .Include(s => s.Street)
                .Include(p =>p.Province)
                .ToList();
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

        public List<AddressModel> GetAddressesInSuburb(Guid suburbId)
        {
            return _dbContext.Addresses.Where(a => a.SuburbId == suburbId)
              .Include(n => n.Suburb)
              .Include(m => m.Municipality)
              .Include(s => s.Street)
              .Include(p => p.Province)
              .ToList();
        }

        public AddressModel FindDuplicate(Guid countryId, Guid provinceId, Guid municipalityId, Guid suburbId, Guid streetId, string streetNumber, Guid? excludeAddressId = null)
        {
            var normalizedStreetNumber = streetNumber?.Trim().ToLower();

            return _dbContext.Addresses.FirstOrDefault(a =>
                a.CountryID == countryId &&
                a.ProvinceID == provinceId &&
                a.MunicipalityID == municipalityId &&
                a.SuburbId == suburbId &&
                a.StreetID == streetId &&
                a.StreetNumber.Trim().ToLower() == normalizedStreetNumber &&
                (excludeAddressId == null || a.Id != excludeAddressId));
        }
    }
}
