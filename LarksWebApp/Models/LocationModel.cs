
using LarksWebApp.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Guid? CategoryId { get; set; }


        public Guid? AddressId { get; set; }

        public MunicipalityModel? Municipality { get; set; }
        public Guid? MunicipalityId { get; set; }

        public SuburbModel? Suburb { get; set; }
        public Guid? SuburbId { get; set; }

        [NotMapped]
        public decimal? Distance { get; set; } = 0;

        public CityModel? City { get; set; }
        public Guid? CityId { get; set; }

        public IList<LocationImageModel> Images { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public bool isReference { get; set; }

        public IList<LocationModelSubcategoryModel> LocalSubcategories { get; set; }

    }
}
