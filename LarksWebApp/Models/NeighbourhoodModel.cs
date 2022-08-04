using LarksCoreApi.Model.BaseModels;
using LarksCoreApi.Models.CoordinatesModel;

namespace LarksCoreApi.Models
{
    public class NeighbourhoodModel :IdentityBaseEntity
    {


        public int MunicipalityId { get; set; }

        public MunicipalityModel Municipality { get; set; }

        public int CityId { get; set; }

        public CityModel CIty { get; set; }

        public IList<NeighbourhoodStreets> NeighbourhoodStreets { get; set; }


        public IList<NeighbourhoodCoordinates> NeighbourhoodCoordinates { get; set; }


        public IList<AddressModel> Addresses { get; set; }
    }
}
