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

        /// <summary>
        /// Finds an existing address at the same Country/Province/Municipality/Suburb/Street/
        /// StreetNumber combination, if one exists. Pass excludeAddressId when checking during
        /// an update so the record being updated doesn't match itself.
        /// </summary>
        AddressModel FindDuplicate(Guid countryId, Guid provinceId, Guid municipalityId, Guid suburbId, Guid streetId, string streetNumber, Guid? excludeAddressId = null);



        public bool DeleteAddress(AddressModel Address);


        public bool UpdateAddress(AddressModel Address);

        bool Save();
    }
}
