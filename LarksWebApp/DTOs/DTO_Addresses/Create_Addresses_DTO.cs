namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class Create_Addresses_DTO
    {

        public int Id { get; set; }
        public int CountryId { get; set; }

        public int ProvinceID { get; set; }

        public int MunicipalityID { get; set; }

        public int NeighbourhoodId { get; set; }
        public int StreetID { get; set; }

        public string StreetNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        public BaseCoordinatesDTO Coordinates { get; set; }
        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        public int BuildingTypeId { get; set; }

        public IList<Create_BuildingType_DTO> ? ListOfBuildingType_DTO  { get; set; }
        //add Address Reference.
    }
}
