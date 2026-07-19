using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;

namespace LarksWebApp.DTOs.DTO_Neighbourhoods
{
    public class Create_suburb_DTO
    {

        public Guid Id { get; set; }
        public Guid MunicipalityId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public string Designation { get; set; }

        public List<DistrictDTO> Districts { get; set; }
        public List<CitiesDTO> Cities { get; set; }
        public List<MunicipalityDTO> Municipalities { get; set; }

        public ProvinceDTO provinceDTO { get; set; }

    }
}
