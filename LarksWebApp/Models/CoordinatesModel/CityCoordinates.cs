using System.ComponentModel.DataAnnotations.Schema;

namespace LarksCoreApi.Models.CoordinatesModel
{
    public class CityCoordinates:BaseCoordinates
    {
        public CityModel City { get; set; }

        [Column(Order = 1)]
        public int CityID { get; set; }
    }
}
