using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.CoordinatesModel;

namespace LarksWebApp.Models
{
    public class StreetModel :IdentityBaseEntity
    {

        public bool IsDeleted { get; set; }

        public IList<SuburbStreets> SuburbLinks { get; set; }

        public IList<StreetCoordinates> StreetCoordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
