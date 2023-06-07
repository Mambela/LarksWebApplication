using LarksWebApp.DTOs.DTO_Provinces;

namespace LarksWebApp.DTOs
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }

        public IList<ProvinceDTO> Provinces { get; set; }
    }

    public class ViewCountryDTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }

        public IList<ViewProvinceDTO> Provinces { get; set; }
    }
}
