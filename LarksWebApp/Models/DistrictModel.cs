using LarksCoreApi.Model.BaseModels;

namespace LarksCoreApi.Models
{
    public class DistrictModel :IdentityBaseEntity
    {
        public ProvinceModel Province { get; set; }

        public int ProvinceId { get; set; }

    }
}
