using LarksCoreApi.Model.BaseModels;
using LarksCoreApi.Models.CoordinatesModel;

namespace LarksCoreApi.Models
{
    public class StreetModel :IdentityBaseEntity
    {

        public IList<NeighbourhoodStreets> NeighbourhoodStreets { get; set; }

        public IList<StreetCoordinates> StreetCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
