
using LarksWebApp.Models.CoordinatesModel;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.Models
{
    public class AddressModel 
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public CountryModel Country { get; set; }
        public Guid? CountryID { get; set; }

        public ProvinceModel Province { get; set; }
        public Guid? ProvinceID { get; set; }

        public MunicipalityModel Municipality { get; set; }
        public Guid? MunicipalityID { get; set; }

        public SuburbModel Suburb { get; set; }
        public Guid? SuburbId { get; set; }


        public StreetModel Street { get; set; }
        public Guid? StreetID { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string? StreetNumber { get; set; }

        public string? Reference { get; set; }

        public string? AdditionalInfo { get; set; }

        public Guid? BuildingTypeId { get; set; }

        public BuildingTypeModel BuildingType { get; set; }

    }
}
