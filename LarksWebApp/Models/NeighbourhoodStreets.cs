using System.ComponentModel.DataAnnotations;

namespace LarksCoreApi.Models
{
    public class NeighbourhoodStreets
    {
        [Key]
        public int StreetsID { get; set; }
        public StreetModel Streets { get; set; }

        public NeighbourhoodModel  Neighbourhood { get; set; }

        [Key]
        public int NeighbourhoodID { get; set; }
    }
}
