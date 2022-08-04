using LarksCoreApi.Model.BaseModels;

namespace LarksCoreApi.Models
{
    public class ProvinceModel :IdentityBaseEntity
    {

        public CountryModel Country { get; set; }

        public int CountryId { get; set; }

        //public IList<MunicipalityModel> Municipalities { get; set; }

        //public IList<DistrictModel> Districts { get; set; }

        //public IList<CityModel> Cities { get; set; }

    }
}
