namespace LarksWebApp.Models
{
    public class LocationImageModel
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImagePath { get; set; }


        public LocationModel Location { get; set; }
        public Guid LocationId { get; set; }


    }
}
