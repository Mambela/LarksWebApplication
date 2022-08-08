using LarksWebApp.Models;


namespace LarksWebApp.Repository.AddressRepository
{
    public interface IAddressRepository
    {
        bool AddressExists(string designation);
        bool AddressExists(int AddressID);

        Task<AddressModel> CreateAddress(AddressModel Address);

        AddressModel GetAddress(int AddressId);

        List<AddressModel> GetAddressesInStreets(int streetsId);

        List<AddressModel> GetAddressesNearLocation(double latitude, double longitude);

        public bool DeleteAddress(AddressModel Address);


        public bool UpdateAddress(AddressModel Address);

        bool Save();
    }
}
