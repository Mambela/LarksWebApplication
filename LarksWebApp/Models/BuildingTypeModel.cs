namespace LarksWebApp.Models
{
    public class BuildingTypeModel
    {
        public int Id { get; set; }

        public string Designation { get; set; }

        public string Lang { get; set; }

        public IList<AddressModel> AddressModels { get; set; }
    }
}
