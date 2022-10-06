using LarksWebApp.DTOs.DTO_Addresses;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class LocationDTO
    {
        public int Id { get; set; }

        public string Designation { get; set; }
        public string AdditionalInformation { get; set; }
        /*
         * Locations may have multiple categories
         * 
         * ***/

        public int CategoryId { get; set; }
        public string Category { get; set; }

        public int SubcategoryId { get; set; }
        public string Subcategory { get; set; }

        public string StreetName { get; set; }


        public ViewAddressDTO  Address { get; set; }

        public int AddressId { get; set; }

        public IList<string> Images { get; set; }
    }
}
