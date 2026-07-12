namespace LarksWebApp.DTOs.DTO_Municipalities
{
    public class Create_Municipality_DTO
    {
        public Guid? Id { get; set; }
        public string Designation { get; set; }
        public Guid ProvinceId { get; set; }


        public IList<BaseCoordinatesDTO>? Points { get; set; }
        public bool IsUpdateCoordinates { get; set; }
    }
}
