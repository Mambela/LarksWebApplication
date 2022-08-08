using LarksWebApp.Model.BaseModels;
 
namespace LarksWebApp.Models
{
    public class CountryModel : IdentityBaseEntity
    {

        public IList<ProvinceModel> Provinces { get; set; }

    }
}
