using LarksWebApp.Services.Loggers;

namespace LarksWebApp.Services.FileManagement
{
    public class FileManagement : IFileManagement
    {

        private readonly ILoggerTrace loggerTrace;

        public FileManagement(ILoggerTrace _loggerTrace)
        {
            loggerTrace = _loggerTrace;
        }

        public async Task <List<string>>MultipleFileUploads(List<IFormFile> Files, string destination, string subfolder = "")
        {
            try
            {

                var uploadedFiles = new List<string>();

                foreach (IFormFile file in Files)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/Images/" + destination + "/" + subfolder);

                    //CREATE FOLDER IF NOT EXIST

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);


                    var newguid = Guid.NewGuid().ToString();
                    var fileName = newguid + new FileInfo(file.FileName).Extension;
                    
                    string fileNameWithPath = Path.Combine(path, fileName);


                  await  using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }
                    uploadedFiles.Add(fileName);
                  //  uploadedFiles.Add(file.FileName);

                }

                return uploadedFiles;

            }
            catch (Exception ex)
            {
                loggerTrace.LogError("Error while trying to upload multiple files " + ex.Message);
                return null;
            }
        }


        public string SingleFileUpload(IFormFile file,string destination,string subfolder="")
        {
            try
            {
                string fileName = "";

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/" + destination+"/"+subfolder);

                if (Directory.Exists(path))
                    Directory.CreateDirectory(path);

                FileInfo fileInfo = new FileInfo(file.FileName);

                fileName = file.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return fileName;

            }
            catch (Exception ex)
            {
                loggerTrace.LogError("Error while uploading a single file " + ex.Message);

                return null;
            }
        }
    }
}
