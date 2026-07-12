using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;

namespace LarksWebApp.DTOs.DTO_Provinces
{
    public class ProvinceDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public Guid CountryId { get; set; }

        

        public IList<MunicipalityDTO> Municipalities { get; set; }

        public IList<DistrictDTO> Districts { get; set; }

        public IList<CitiesDTO> Cities { get; set; }
    }


    public class ViewProvinceDTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public Guid CountryId { get; set; }
        public IList<MunicipalityDTO> Municipalities { get; set; }
        public IList<DistrictDTO> Districts { get; set; }

        public IList<CitiesDTO> Cities { get; set; }
    }
}
