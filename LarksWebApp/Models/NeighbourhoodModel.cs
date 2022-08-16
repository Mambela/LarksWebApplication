using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class NeighbourhoodModel :IdentityBaseEntity
    {


        public int MunicipalityId { get; set; }

        public MunicipalityModel Municipality { get; set; }

        public int? CityId { get; set; }

        public CityModel City { get; set; }

        public int? DistrictId { get; set; }
        public DistrictModel District { get; set; }

        public IList<NeighbourhoodStreets> NeighbourhoodStreets { get; set; }


        public IList<NeighbourhoodCoordinates> NeighbourhoodCoordinates { get; set; }


        public IList<AddressModel> Addresses { get; set; }
    }
}
