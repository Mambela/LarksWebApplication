namespace LarksWebApp.DTOs.DTO_City
{
    public class Create_City_DTO
    {

        public int Id { get; set; }
        public int ProvinceId { get; set; }

        public string Designation { get; set; }

        public int CountryId { get; set; }

        public IList<CountryDTO>? ListofCountryDTO { get; set; }


        public IList<BaseCoordinatesDTO>? Points { get; set; }
        public Boolean IsUpdateCoordinates { get; set; }

    }
}
