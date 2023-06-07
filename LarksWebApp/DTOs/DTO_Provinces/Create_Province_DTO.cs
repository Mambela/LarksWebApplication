namespace LarksWebApp.DTOs.DTO_Provinces
{
    public class Create_Province_DTO
    {
        public int Id { get; set; }
        public string Designation { get; set; }

        public int CountryId { get; set; }

        public IList<BaseCoordinatesDTO>? Points { get; set; }

        public Boolean IsUpdateCoordinates { get; set; }
    }
}
