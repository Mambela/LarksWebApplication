
using LarksCoreApi.Models.CoordinatesModel;

namespace LarksCoreApi.Models
{
    public class AddressModel : BaseCoordinates
    {

        public CountryModel Country { get; set; }
        public int CountryID { get; set; }

        public ProvinceModel Province { get; set; }
        public int ProvinceID { get; set; }

        public MunicipalityModel Municipality { get; set; }
        public int MunicipalityID { get; set; }


        public NeighbourhoodModel Neighbourhood { get; set; }
        public int NeighbourhoodId { get; set; }


        public StreetModel Street { get; set; }
        public int StreetID { get; set; }

        public string UnitReference { get; set; }

    }
}
