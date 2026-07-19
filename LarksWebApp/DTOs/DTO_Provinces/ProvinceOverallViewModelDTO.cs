

using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Suburbs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LarksWebApp.DTOs.DTO_Provinces
{
    public class ProvinceOverallViewModelDTO
    {

        public Guid ProvinceId { get; set; }

        public string Designation { get; set; }

        public Guid CityId { get; set; }

        public List<DistrictDTO> Districts { get; set; }
        public List<CitiesDTO> Cities { get; set; }
        public List<MunicipalityDTO> Municipalities { get; set; }

        public Create_suburb_DTO CreateSuburb { get; set; }
    }
}
