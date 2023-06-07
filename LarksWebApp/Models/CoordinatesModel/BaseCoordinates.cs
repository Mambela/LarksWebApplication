using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class BaseCoordinates : TimeStampsEntity
    {

        public int Id { get; set; }

        public decimal lat { get; set; }

        public decimal lng { get; set; }

    }
}

