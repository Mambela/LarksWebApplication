using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class SuburbCoordinates : BaseCoordinates
    {
        [Column(Order=1)]
        public Guid SuburbID { get; set; }
        public SuburbModel Suburb { get; set; }

    }
}
