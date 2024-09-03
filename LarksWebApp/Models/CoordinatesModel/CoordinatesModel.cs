namespace LarksWebApp.Models.CoordinatesModel
{
    public class ReceivedCoordinatesModel
    {

        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
    
    public class ReturnCoordinatesModel
    {

        public int Id { get; set; }

        public int SuburbId { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
  

    public class MappedLocation {

        public int Id { get; set; }

        public int SuburbId { get; set; }
      

        public int MunicipalityId { get; set; }

        public int CityId { get; set; }

        public int SuburbName { get; set; }
    }
}
