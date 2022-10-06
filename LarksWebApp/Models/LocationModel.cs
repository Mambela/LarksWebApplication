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
        public CategoryModel Category { get; set; }
        public int CategoryId { get; set; }


        public SubcategoryModel Subcategory { get; set; }
        public int SubcategoryId { get; set; }


        public AddressModel Address { get; set; }
        public int AddressId { get; set; }

        public IList<LocationImageModel> Images { get; set; }

    }
}
