using LarksWebApp.Models;


namespace LarksWebApp.Repository.AddressRepository.Address_Repository
{
    public interface IAddressRepository
    {
        bool AddressExists(string designation);
        bool AddressExists(Guid AddressID);

        Task<AddressModel> CreateAddress(AddressModel Address);

        AddressModel GetAddress(Guid AddressId);

        List<AddressModel> GetAddressesInStreets(Guid streetsId);

        List<AddressModel> GetAddressesNearLocation(double latitude, double longitude);

        List<AddressModel> GetAddressesInSuburb(Guid suburbId);

        

        public bool DeleteAddress(AddressModel Address);


        public bool UpdateAddress(AddressModel Address);

        bool Save();
    }
}
