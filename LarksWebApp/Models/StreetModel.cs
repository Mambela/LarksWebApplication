using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class StreetModel :IdentityBaseEntity
    {

        public IList<NeighbourhoodStreets> NeighbourhoodStreets { get; set; }

        public IList<StreetCoordinates> StreetCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
