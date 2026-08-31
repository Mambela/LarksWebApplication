using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Suburbs
{
    public class CreateSuburbDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please select a municipality")]
        public Guid MunicipalityId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? DistrictId { get; set; }

        [Required(ErrorMessage = "Please provide a designation for this suburb")]
        public string Designation { get; set; }
    }
}
