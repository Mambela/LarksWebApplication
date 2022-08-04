using LarksCoreApi.Model.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarksCoreApi.Models
{
    public class CityModel : IdentityBaseEntity

    {

        [Column(Order = 3)]
        public int ProvinceId { get; set; }

        public ProvinceModel Province { get; set; }

        public IList<NeighbourhoodModel> Neighbourhoods { get; set; }
    }
}
