using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class Create_Location_DTO
    {

        [Required(ErrorMessage = "Please enter a designation for this location")]
        public string Designation { get; set; }

        public  List<CountryDTO> Country { get; set; }
        public int CountryId { get; set; }

        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        //public int ProvinceID { get; set; }

        //public int MunicipalityID { get; set; }

        //public int NeighbourhoodId { get; set; }
        //public int StreetID { get; set; }

        //public int AddressId { get; set; }


        //public int SubCategoryId { get; set; }


        [Required(ErrorMessage = "Please select files")]
        public List<IFormFile> Files { get; set; }

        [Required(ErrorMessage = "Please add additional Information")]
        public string AdditionalInfo { get; set; }


        public int CategoryId { get; set; }

        public IList<CategoryDTO> ListOfCategories { get; set; }

        public int searchCountryId_hidden { get; set; }
        public int searchProvince_hidden { get; set; }
        public int searchMunicipality_hidden { get; set; }
        public int searchDistrict_hidden { get; set; }
        public int searchNeighbourhood_hidden { get; set; }
        public int searchStreet_hidden { get; set; }


        
        [Required(ErrorMessage = "Please select an address for this location")]
        public int searchAddress_hidden { get; set; }

        [Required(ErrorMessage = "Please add category for this location")]
        public int searchCategoryId_hidden { get; set; }

        [Required(ErrorMessage = "Please select a subcategory")]
        public int searchSubcategoryId_hidden { get; set; }

    }
}
