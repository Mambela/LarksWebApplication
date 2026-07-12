using Microsoft.AspNetCore.Identity;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class ViewAddressDTO
    {
        public Guid Id { get; set; }


        public string Province { get; set; }
        public Guid ProvinceID { get; set; }

        public string Municipality { get; set; }
        public Guid MunicipalityID { get; set; }


        public string Neighbourhood { get; set; }
        public Guid NeighbourhoodId { get; set; }


        public string StreetName { get; set; }
        public Guid StreetID { get; set; }

        public string StreetNumber { get; set; }
        public string FullAddress { get; set; }

        public BaseCoordinatesDTO Coordinates { get; set; }

        
    }
}
