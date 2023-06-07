
using LarksWebApp.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.Models
{
    public class LocationModel : IdentityBaseEntity
    {
        
        
        [StringLength(260)]
        public string AdditionalInformation { get; set; }
        /*
         * Locations may have multiple categories
         * 
         * ***/
        public CategoryModel? Category { get; set; }
        public int? CategoryId { get; set; }

 
        public int? AddressId { get; set; }

        public MunicipalityModel? Municipality { get; set; }
        public int? MunicipalityId { get; set; }

        public SuburbModel? Suburb { get; set; }
        public int? SuburbId { get; set; }

        public virtual decimal ? Distance { get; set; }

        public CityModel? City { get; set; }
        public int? CityId { get; set; }

        public IList<LocationImageModel> Images { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public IList<LocationModelSubcategoryModel> LocalSubcategories { get; set; }

    }
}
