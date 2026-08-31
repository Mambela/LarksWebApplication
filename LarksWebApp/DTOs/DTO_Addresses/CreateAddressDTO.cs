using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class CreateAddressDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please select a country")]
        public Guid CountryId { get; set; }

        [Required(ErrorMessage = "Please select a province")]
        public Guid ProvinceID { get; set; }

        [Required(ErrorMessage = "Please select a municipality")]
        public Guid MunicipalityID { get; set; }

        [Required(ErrorMessage = "Please select a suburb")]
        public Guid SuburbId { get; set; }

        [Required(ErrorMessage = "Please select a street")]
        public Guid StreetID { get; set; }

        [Required(ErrorMessage = "Please provide a street number")]
        [StringLength(10, MinimumLength = 1)]
        public string StreetNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Please select a building type")]
        public Guid BuildingTypeId { get; set; }
    }
}
