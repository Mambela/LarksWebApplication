using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class NeighbourhoodCoordinates : BaseCoordinates
    {
        [Column(Order=1)]
        public int NeighbourhoodID { get; set; }
        public NeighbourhoodModel Neighbourhood { get; set; }

    }
}
