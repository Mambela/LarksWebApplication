namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class Create_Addresses_DTO
    {

        public Guid Id { get; set; }
        public Guid CountryId { get; set; }

        public Guid ProvinceID { get; set; }

        public Guid MunicipalityID { get; set; }

        public Guid NeighbourhoodId { get; set; }
        public Guid StreetID { get; set; }

        public string StreetNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        public BaseCoordinatesDTO Coordinates { get; set; }

        public double lat { get; set; }

        public double lng { get; set; }

        public IList<CountryDTO>? ListofCountryDTO { get; set; }

        public Guid BuildingTypeId { get; set; }

        public IList<Create_BuildingType_DTO> ? ListOfBuildingType_DTO  { get; set; }
        //add Address Reference.
    }
}
