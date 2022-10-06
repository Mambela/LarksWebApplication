namespace LarksWebApp.DTOs.DTO_Country
{
    public class ViewListCountryDTO
    {
        public int CountryId { get; set; }
        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        public int? searchAddress_hidden { get; set; }
    }
}
