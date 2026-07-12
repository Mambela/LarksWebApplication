using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class Create_Location_DTO
    {

        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Please enter a designation for this location")]
        public string Designation { get; set; }

        public  List<CountryDTO>? Country { get; set; }
        public Guid CountryId { get; set; }

        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        public Guid MunicipalityID { get; set; }

        public Guid SuburbId { get; set; }
        public Guid CityId { get; set; }

        public IList<Guid> SubCategories { get; set; }

        
        public List<IFormFile>? Files { get; set; }

        [Required(ErrorMessage = "Please add additional Information")]
        public string AdditionalInfo { get; set; }

        

        public Guid CategoryId { get; set; }

        public IList<CategoryDTO>? ListOfCategories { get; set; }

        #region Hidden Values
        public Guid searchCountryId_hidden { get; set; }
        public Guid searchProvince_hidden { get; set; }
        public Guid searchMunicipality_hidden { get; set; }
        public Guid searchDistrict_hidden { get; set; }
        public Guid searchNeighbourhood_hidden { get; set; }
        public Guid searchStreet_hidden { get; set; }



        [Required(ErrorMessage = "Please select an address for this location")]
        public Guid searchAddress_hidden { get; set; }

        [Required(ErrorMessage = "Please add category for this location")]
        public Guid searchCategoryId_hidden { get; set; }

        [Required(ErrorMessage = "Please select a subcategory")]
        public Guid searchSubcategoryId_hidden { get; set; }
        #endregion
        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

    }
}
