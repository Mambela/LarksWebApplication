using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Suburbs;
using LarksWebApp.DTOs.DTO_Provinces;

namespace LarksWebApp.DTOs.DTO_Streets
{
    public class Create_Street_DTO 
    {

        public Guid Id { get; set; }
        public string Designation { get; set; }

        public IList<Guid> SuburbID { get; set; }

        public Guid ? CountryId  { get; set; }

        public IList<CountryDTO> ? ListofCountryDTO { get; set; }
        public IList<ProvinceDTO> ? ListofProvinceDTO { get; set; }
        public IList<CitiesDTO> ? ListofCityDTO { get; set; }


        public IList<DistrictDTO> ? ListofDistrictDTO { get; set; }
        public IList<MunicipalityDTO> ? ListofMunicipalityDTO { get; set; }
        public IList<SuburbDTO> ? ListofSuburbDTO { get; set; }
        public IList<BaseCoordinatesDTO> ? Coordinates{ get; set; }



    }
}
