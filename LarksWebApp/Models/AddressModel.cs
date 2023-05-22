
using LarksWebApp.Models.CoordinatesModel;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.Models
{
    public class AddressModel : BaseCoordinates
    {

        public CountryModel Country { get; set; }
        public int? CountryID { get; set; }

        public ProvinceModel Province { get; set; }
        public int ProvinceID { get; set; }

        public MunicipalityModel Municipality { get; set; }
        public int MunicipalityID { get; set; }

        public SuburbModel Neighbourhood { get; set; }
        public int NeighbourhoodId { get; set; }


        public StreetModel Street { get; set; }
        public int StreetID { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string StreetNumber { get; set; }

        public string? Reference { get; set; }

        public string? AdditionalInfo { get; set; }

        public int BuildingTypeId { get; set; }

        public BuildingTypeModel BuildingType { get; set; }

    }
}
