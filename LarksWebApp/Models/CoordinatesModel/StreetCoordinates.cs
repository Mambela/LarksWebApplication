using System.ComponentModel.DataAnnotations.Schema;

namespace LarksCoreApi.Models.CoordinatesModel
{
    public class StreetCoordinates :BaseCoordinates
    {

        [Column(Order=1)]
        public int StreetId { get; set; }

        public StreetModel Street { get; set; }
    }
}
