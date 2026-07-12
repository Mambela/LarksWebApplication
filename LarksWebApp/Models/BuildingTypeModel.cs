namespace LarksWebApp.Models
{
    public class BuildingTypeModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Designation { get; set; }

        public string Lang { get; set; }

        public IList<AddressModel> AddressModels { get; set; }
    }
}
