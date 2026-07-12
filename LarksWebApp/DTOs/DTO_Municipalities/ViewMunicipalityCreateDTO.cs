namespace LarksWebApp.DTOs.DTO_Municipalities
{
    public class ViewMunicipalityCreateDTO
    {
        public Guid ProviceId { get; set; }

        public List<MunicipalityDTO> MunicipalityDTO { get; set; }
    }
}
