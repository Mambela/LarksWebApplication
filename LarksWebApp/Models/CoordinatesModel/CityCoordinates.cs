using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class CityCoordinates:BaseCoordinates
    {
        public CityModel City { get; set; }

        [Column(Order = 1)]
        public int CityID { get; set; }
    }
}
