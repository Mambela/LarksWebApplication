using LarksCoreApi.Model.BaseModels;
using LarksCoreApi.Models.CoordinatesModel;

namespace LarksCoreApi.Models
{
    public class MunicipalityModel :IdentityBaseEntity
    {

        public ProvinceModel Province { get; set; }

        public int ProvinceId { get; set; }

        public IList<NeighbourhoodModel> Neighbourhoods { get; set; }

        public IList<MunicipalityCoordinates> ListCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }


    }
}
