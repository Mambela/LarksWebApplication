using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class SuburbModel :IdentityBaseEntity
    {

        public bool IsDeleted { get; set; }

        public Guid MunicipalityId { get; set; }

        public MunicipalityModel Municipality { get; set; }

        public Guid? CityId { get; set; }

        public CityModel City { get; set; }

        public bool ReviewName { get; set; } = false;

        public Guid? DistrictId { get; set; }
        public DistrictModel District { get; set; }

        public IList<SuburbStreets> StreetLinks { get; set; }

        public IList<SuburbCoordinates> Coordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
