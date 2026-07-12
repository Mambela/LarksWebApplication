using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models
{
    public class DistrictModel :IdentityBaseEntity
    {
        public ProvinceModel Province { get; set; }

        public Guid ProvinceId { get; set; }

    }
}
