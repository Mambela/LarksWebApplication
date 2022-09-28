namespace LarksWebApp.Services.FileManagement
{
    public interface IFileManagement
    {
        Task <List<string>> MultipleFileUploads(List<IFormFile> Files, string destination, string subfolder);

        string SingleFileUpload(IFormFile File, string destination, string subfolder = "");

    }
}
