using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class CreateAddressDTO : IValidatableObject
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

        /// <summary>
        /// Either this or <see cref="StreetName"/> must be provided. When StreetName is
        /// supplied it takes priority: the street is looked up by name (reusing it if it
        /// already exists) and this field is ignored.
        /// </summary>
        public Guid? StreetID { get; set; }

        /// <summary>
        /// Optional. When provided, the street is resolved by name before the address is
        /// created - reusing the existing street if one with this name already exists,
        /// otherwise creating it and linking it to the given Suburb.
        /// </summary>
        public string? StreetName { get; set; }

        [Required(ErrorMessage = "Please provide a street number")]
        [StringLength(10, MinimumLength = 1)]
        public string StreetNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Please select a building type")]
        public Guid BuildingTypeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((StreetID == null || StreetID == Guid.Empty) && string.IsNullOrWhiteSpace(StreetName))
            {
                yield return new ValidationResult(
                    "Please provide either a streetId or a streetName.",
                    new[] { nameof(StreetID), nameof(StreetName) });
            }
        }
    }
}
