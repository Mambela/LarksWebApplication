using LarksWebApp.Model.BaseModels;

namespace LarksWebApp.Models.CoordinatesModel
{
    public class BaseCoordinates : TimeStampsEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal lat { get; set; }

        public decimal lng { get; set; }

    }
}

