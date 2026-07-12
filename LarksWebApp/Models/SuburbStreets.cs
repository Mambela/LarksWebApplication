using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.Models
{
    public class SuburbStreets
    {
        [Key]
        public Guid StreetsID { get; set; }
        public StreetModel Streets { get; set; }

        public SuburbModel  Suburb { get; set; }

        [Key]
        public Guid SuburbId { get; set; }
    }
}
