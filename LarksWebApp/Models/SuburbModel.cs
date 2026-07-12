using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class SuburbModel :IdentityBaseEntity
    {


        public Guid? MunicipalityId { get; set; }

        public MunicipalityModel Municipality { get; set; }

        public Guid? CityId { get; set; }

        public CityModel City { get; set; }

        public bool ReviewName { get; set; } = false;

        public Guid? DistrictId { get; set; }
        public DistrictModel District { get; set; }

        public IList<SuburbStreets> NeighbourhoodStreets { get; set; }

        public IList<SuburbCoordinates> NeighbourhoodCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
