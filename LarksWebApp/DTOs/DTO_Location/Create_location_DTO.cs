using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;

namespace LarksWebApp.DTOs.DTO_Location
{
    public class Create_Location_DTO
    {
        public int CountryId { get; set; }

        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        //public int ProvinceID { get; set; }

        //public int MunicipalityID { get; set; }

        //public int NeighbourhoodId { get; set; }
        //public int StreetID { get; set; }

        //public int AddressId { get; set; }


        //public int SubCategoryId { get; set; }

        public List<IFormFile> Files { get; set; }

        public string AdditionalInfo { get; set; }

        public int CategoryId { get; set; }

        public IList<CategoryDTO> ListOfCategories { get; set; }

        public int searchCountryId_hidden { get; set; }
        public int searchProvince_hidden { get; set; }
        public int searchMunicipality_hidden { get; set; }
        public int searchDistrict_hidden { get; set; }
        public int searchNeighbourhood_hidden { get; set; }
        public int searchStreet_hidden { get; set; }
        public int searchAddress_hidden { get; set; }
        public int searchCategoryId_hidden { get; set; }
        public int searchSubcategoryId_hidden { get; set; }
   
    }
}
