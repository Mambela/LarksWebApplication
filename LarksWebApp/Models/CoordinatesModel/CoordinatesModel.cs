namespace LarksWebApp.Models.CoordinatesModel
{
    public class ReceivedCoordinatesModel
    {

        public Guid Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }

    public class ReturnCoordinatesModel
    {

        public Guid Id { get; set; }

        public Guid SuburbId { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }


    public class MappedLocation {

        public Guid Id { get; set; }

        public Guid SuburbId { get; set; }


        public Guid MunicipalityId { get; set; }

        public Guid CityId { get; set; }

        public int SuburbName { get; set; }
    }
}
