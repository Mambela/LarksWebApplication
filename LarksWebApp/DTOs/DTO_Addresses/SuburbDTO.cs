using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class SuburbDTO
    {
        public Guid Id { get; set; }

        public string Designation { get; set; }

        public string CityName { get; set; }

        public string MunicipalityName { get; set; }
    }
}
