using System.ComponentModel.DataAnnotations.Schema;

namespace LarksCoreApi.Models.CoordinatesModel
{
    public class ProvinceCoordinates :BaseCoordinates
    {

        public ProvinceModel Province { get; set; }

        [Column(Order = 1)]
        public int ProvinceId { get; set; }

       
    }
}
