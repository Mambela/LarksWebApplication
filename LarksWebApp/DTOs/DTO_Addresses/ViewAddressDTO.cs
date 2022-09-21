namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class ViewAddressDTO
    {
        public int Id { get; set; }


        public string Province { get; set; }
        public int ProvinceID { get; set; }

        public string Municipality { get; set; }
        public int MunicipalityID { get; set; }


        public string Neighbourhood { get; set; }
        public int NeighbourhoodId { get; set; }


        public string StreetName { get; set; }
        public int StreetID { get; set; }

        public string StreetNumber { get; set; }
        public string FullAddress { get; set; }
    }
}
