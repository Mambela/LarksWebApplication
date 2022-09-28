namespace LarksWebApp.Models
{
    public class LocationImageModel
    {

        public int Id { get; set; }
        public string ImagePath { get; set; }


        public LocationModel Location { get; set; }
        public int LocationId { get; set; }


    }
}
