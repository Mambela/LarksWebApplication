using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class LocationDTO
    {
        public Guid Id { get; set; }

        public string Designation { get; set; }
        public string AdditionalInformation { get; set; }
        /*
         * Locations may have multiple categories
         *
         * ***/

        public Guid CategoryId { get; set; }
        public string Category { get; set; }

        public Guid SubcategoryId { get; set; }
        public  IEnumerable<SubCategoryDTO> Subcategory { get; set; }

        public string StreetName { get; set; }

        public decimal? Distance { get; set; }
        public ViewAddressDTO  Address { get; set; }

        public Guid AddressId { get; set; }

        public IList<string> Images { get; set; }

        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
