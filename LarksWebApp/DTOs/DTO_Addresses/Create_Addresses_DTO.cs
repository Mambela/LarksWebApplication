namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class Create_Addresses_DTO
    {        
        public int CountryID { get; set; }

        public int ProvinceID { get; set; }

        public int MunicipalityID { get; set; }

        public int NeighbourhoodId { get; set; }
        public int StreetID { get; set; }

        public string UnitReference { get; set; }
    }
}
