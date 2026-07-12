namespace LarksWebApp.Models
{
    public class LocationObj
    {
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

        public decimal? Distance { get; set; }

        public CityModel? City { get; set; }
        public Guid? CityId { get; set; }

        public IList<LocationImageModel> Images { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public IList<LocationModelSubcategoryModel> LocalSubcategories { get; set; }
    }
}
