using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class MunicipalityModel :IdentityBaseEntity
    {

        public ProvinceModel Province { get; set; }

        public Guid ProvinceId { get; set; }

        public IList<SuburbModel> Neighbourhoods { get; set; }

        public IList<MunicipalityCoordinates> ListCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }


    }
}
