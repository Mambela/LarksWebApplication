using LarksWebApp.Model.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Models
{
    public class CityModel : IdentityBaseEntity

    {

        [Column(Order = 3)]
        public Guid ProvinceId { get; set; }

        public ProvinceModel Province { get; set; }

        public IList<SuburbModel> Neighbourhoods { get; set; }
    }
}
