using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Models;

namespace LarksWebApp.DTOs.DTO_Streets
{
    public class View_Street_DTO
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }

        public IList<SuburbDTO> Neighbourhoods { get; set; }

        public IList<BaseCoordinatesDTO> Coordinates { get; set; }

        public IList<AddressModel> Addresses { get; set; }
    }
}
