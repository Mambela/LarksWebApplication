using LarksCoreApi.Model.BaseModels;
 
namespace LarksCoreApi.Models
{
    public class CountryModel : IdentityBaseEntity
    {

        public IList<ProvinceModel> Provinces { get; set; }

    }
}
