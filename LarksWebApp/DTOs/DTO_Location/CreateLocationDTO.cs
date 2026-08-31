using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class CreateLocationDTO
    {
        [Required(ErrorMessage = "Please enter a designation for this location")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Please add category for this location")]
        public Guid CategoryId { get; set; }

 
        public Guid ? AddressId  { get; set; }

        [Required(ErrorMessage = "Please select a municipality for this location")]
        public Guid MunicipalityId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? SuburbId { get; set; }

        public IList<Guid>? SubCategories { get; set; }

        [Required(ErrorMessage = "Please add additional Information")]
        public string AdditionalInfo { get; set; }

        [Required]
        public decimal Lat { get; set; }

        [Required]
        public decimal Lng { get; set; }
    }
}
