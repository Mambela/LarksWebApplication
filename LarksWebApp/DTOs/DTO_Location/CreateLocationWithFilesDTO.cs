namespace LarksWebApp.DTOs.DTO_Location
{
    public class CreateLocationWithFilesDTO : CreateLocationDTO
    {
        public List<IFormFile>? Files { get; set; }
    }
}
