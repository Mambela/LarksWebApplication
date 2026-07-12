using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class SuburbDTO
    {
        public Guid Id { get; set; }

        public string Designation { get; set; }
    }
}
