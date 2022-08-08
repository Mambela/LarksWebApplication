using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class StreetCoordinates :BaseCoordinates
    {

        [Column(Order=1)]
        public int StreetId { get; set; }

        public StreetModel Street { get; set; }
    }
}
