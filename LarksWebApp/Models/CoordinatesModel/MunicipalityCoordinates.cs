using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class MunicipalityCoordinates : BaseCoordinates
    {
        public MunicipalityModel Municipality { get; set; }

        [Column(Order=1)]
        public Guid MunicipalityID { get; set; }


    }
}
