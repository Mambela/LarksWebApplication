using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class MunicipalityModel :IdentityBaseEntity
    {

        public bool IsDeleted { get; set; }

        public ProvinceModel Province { get; set; }

        public Guid ProvinceId { get; set; }

        public IList<SuburbModel> Suburbs { get; set; }

        public IList<MunicipalityCoordinates> ListCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }


    }
}
