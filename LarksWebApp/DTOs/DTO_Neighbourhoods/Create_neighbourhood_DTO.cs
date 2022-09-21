using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;

namespace LarksWebApp.DTOs.DTO_Neighbourhoods
{
    public class Create_neighbourhood_DTO
    {

        public int Id { get; set; }
        public int MunicipalityId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Designation { get; set; }

        public List<DistrictDTO> Districts { get; set; }
        public List<CitiesDTO> Cities { get; set; }
        public List<MunicipalityDTO> Municipalities { get; set; }

        public ProvinceDTO provinceDTO { get; set; }

    }
}
