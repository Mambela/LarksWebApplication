using LarksWebApp.Models;


namespace LarksWebApp.Repository.AddressRepository.Address_Repository
{
    public interface IAddressRepository
    {
        bool AddressExists(string designation);
        bool AddressExists(int AddressID);

        Task<AddressModel> CreateAddress(AddressModel Address);

        AddressModel GetAddressInStreet(int AddressId);

        List<AddressModel> GetAddressesInStreets(int streetsId);

        List<AddressModel> GetAddressesNearLocation(double latitude, double longitude);

        List<AddressModel> GetAddressesInNeighbourhood(int neighbourhoodId);

        public bool DeleteAddress(AddressModel Address);


        public bool UpdateAddress(AddressModel Address);

        bool Save();
    }
}
