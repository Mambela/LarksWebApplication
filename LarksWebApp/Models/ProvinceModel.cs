using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class ProvinceModel :IdentityBaseEntity
    {

        public CountryModel Country { get; set; }

        public Guid CountryId { get; set; }

        public IList<MunicipalityModel> Municipalities { get; set; }

        public IList<DistrictModel> Districts { get; set; }

        public IList<CityModel> Cities { get; set; }

        //public IList<BaseCoordinatesDTO> Points { get; set; }
    }
}
