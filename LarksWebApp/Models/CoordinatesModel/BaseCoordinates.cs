using LarksCoreApi.Model.BaseModels;

namespace LarksCoreApi.Models.CoordinatesModel
{
    public class BaseCoordinates : TimeStampsEntity
    {

        public int Id { get; set; }

        public double lat { get; set; }

        public double lng { get; set; }

    }
}
