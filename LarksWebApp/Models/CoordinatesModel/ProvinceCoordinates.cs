using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class ProvinceCoordinates :BaseCoordinates
    {

        public ProvinceModel Province { get; set; }

        [Column(Order = 1)]
        public Guid ProvinceId { get; set; }

       
    }
}
