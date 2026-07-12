namespace LarksWebApp.DTOs.DTO_Country
{
    public class ViewListCountryDTO
    {
        public Guid CountryId { get; set; }
        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        public Guid? searchAddress_hidden { get; set; }
    }
}
