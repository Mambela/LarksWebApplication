using LarksWebApp.DTOs.DTO_Provinces;

namespace LarksWebApp.DTOs
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }

        public IList<ProvinceDTO> ProvincesDTO { get; set; }
    }
}
