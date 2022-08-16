using LarksWebApp.Model.BaseModels;
 
namespace LarksWebApp.Models
{
    public class CountryModel : IdentityBaseEntity
    {

        public List<ProvinceModel> Provinces { get; set; }

    }
}
