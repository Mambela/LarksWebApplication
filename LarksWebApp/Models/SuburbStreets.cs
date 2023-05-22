using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.Models
{
    public class SuburbStreets
    {
        [Key]
        public int StreetsID { get; set; }
        public StreetModel Streets { get; set; }

        public SuburbModel  Neighbourhood { get; set; }

        [Key]
        public int SuburbId { get; set; }
    }
}
