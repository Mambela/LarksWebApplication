using LarksWebApp.DTOs;

namespace LarksWebApp.Services.FileManagement
{
    public interface IFileManagement
    {
        Task <List<string>> MultipleFileUploads(List<IFormFile> Files, string destination, string subfolder);

        string SingleFileUpload(IFormFile File, string destination, string subfolder = "");

        Task<List<MediaResponse>> SaveMedia(List<IFormFile> files,string baseDestination);

        MediaResponse ValidateMedia(IFormFile file);

        MediaResponse ValidateImageStream(IFormFile file);

    }
}
