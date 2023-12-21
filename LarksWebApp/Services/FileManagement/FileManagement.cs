using LarksWebApp.DTOs;
using LarksWebApp.Services.Loggers;

namespace LarksWebApp.Services.FileManagement
{
    public class FileManagement : IFileManagement
    {

        private readonly ILoggerTrace loggerTrace;
        private const int ImageMaxSize = 2620;
        public readonly IWebHostEnvironment webHostEnvironment;

        public FileManagement(ILoggerTrace _loggerTrace,IWebHostEnvironment _webHost)
        {
            loggerTrace = _loggerTrace;
            webHostEnvironment = _webHost;
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

        public async Task<List<MediaResponse>> SaveMedia(List<IFormFile> files,string baseDestination)
        {
            var uploadMediaResponses = new List<MediaResponse>();

            if (files == null)
            {
                loggerTrace.Warning("No files to save. file list is empty.");
                return uploadMediaResponses;
            }

            //Resources path. 
            string resourceRoot = "Files"; 
            //string resourceRoot = "\\resources",type="\\multimedia\\";


            loggerTrace.Verbose("Looping through files to save.");

            foreach (IFormFile file in files)
            {

                try
                {
                    var mediaResponse = new MediaResponse();

                    mediaResponse = ValidateMedia(file);



                    if (ValidateMedia(file).isSucess || ValidateImageStream(file).isSucess)
                    {

                        //upload Media here

                        if (file.Length > 0)
                        {
                            //string _root =Path.Combine(Directory.GetCurrentDirectory(), webHostEnvironment.WebRootPath + resourceRoot + type);
                            string _root = "";
                            _root = Path.Combine(Directory.GetCurrentDirectory(), webHostEnvironment.WebRootPath);
                            _root = Path.Combine(_root, resourceRoot);
                            _root = Path.Combine(_root, "Images");
                            _root = Path.Combine(_root, baseDestination);                          



                            if (!Directory.Exists(_root))
                            {
                                Directory.CreateDirectory(_root);
                                loggerTrace.Verbose("Directory cerated");
                            }

                            Guid guid = Guid.NewGuid();
                            FileInfo fileInfo = new FileInfo(file.FileName);

                            string filePath = guid.ToString() + fileInfo.Extension;


                            using (var fileStream = new FileStream(Path.Combine(_root, filePath), FileMode.CreateNew))
                            {
                                await file.CopyToAsync(fileStream);
                                fileStream.Flush();


                                mediaResponse.isSucess = true;
                                mediaResponse.imageMessage = filePath;


                                // Add it to the Response List
                            }
                            loggerTrace.Verbose($"File {file.FileName} saved successfuly");
                            uploadMediaResponses.Add(mediaResponse);
                        }
                    }


                }
                catch (Exception ex)
                {
                    //log the data here. 
                    loggerTrace.LogError($"Error occured trying to save file {file.FileName}");
                    var exception = ex.Message;
                }

            }

            return uploadMediaResponses;
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


        public MediaResponse ValidateMedia(IFormFile file)
        {

            loggerTrace.Verbose($"Validating {file.FileName}");

            string extension = file.ContentType; long fileSize = file.Length;
            MediaResponse uploadResponse = new MediaResponse();

            /*
             * Retrieve from the end of the extension to the an / so that we get just the type of the image
             * **/

            loggerTrace.Verbose($"Retrive extension for {file.FileName}");
            extension = extension.Substring(extension.IndexOf("/") + 1, extension.Length - (extension.IndexOf("/") + 1));

            //_mediaResponse.Message = extension;


            var supportedImages = new[] { "png", "jpg", "jpeg" };
            var supportedVideo = new[] { "video/avi", "video/flv", "video/mp4", "video/webm", "avi", "flv", "mp4", "webm" };

            if (supportedImages.Contains(extension))
            {
                //Maximum size allowed is 2.6 Mb
                loggerTrace.Verbose($"Validating {file.FileName} extenstion against supported list of image extensions");

                if (fileSize / 4192 > ImageMaxSize)
                {
                    uploadResponse.isSucess = false;
                    uploadResponse.imageMessage = "Error!  The file size is too large";
                    loggerTrace.LogError($"Error! {file.FileName} file size is too large.");

                    return uploadResponse;
                }


                uploadResponse.imageMessage = "Image has passed all the validations ";
                uploadResponse.isSucess = true;
                uploadResponse.mediaType = "image";

                loggerTrace.Verbose($"Image file {file.FileName} has passed all validations");
                //Check if the File size is within size limit. 


                // Check if the file extension is within documents required. 
                //Check if the file is a jpeg, jpg or Png. 


                return uploadResponse;

            }
            else if (supportedVideo.Contains(extension))
            {
                loggerTrace.Verbose($"Validating {file.FileName} extenstion against supported list of video extensions");

                if (fileSize / 419200 > ImageMaxSize)
                {
                    uploadResponse.isSucess = false;
                    uploadResponse.imageMessage = "Error!  The video size is too large";
                    loggerTrace.LogError($"Error! {file.FileName} file size is too large.");

                    return uploadResponse;
                }

                uploadResponse.imageMessage = "Video has passed all the validations ";
                uploadResponse.isSucess = true;
                uploadResponse.mediaType = "video";

                loggerTrace.Verbose($"Video file {file.FileName} has passed all validations");

                return uploadResponse;
            }


            uploadResponse.isSucess = false;
            uploadResponse.imageMessage = "Error! Media provided not supported";
            loggerTrace.LogError($"Error! {file.FileName} not supported.");

            return uploadResponse;
        }

        public MediaResponse ValidateImageStream(IFormFile postedFile)
        {

            loggerTrace.Verbose($"Validating {postedFile.FileName} image stream");

            MediaResponse response = new MediaResponse();
            using var stream = postedFile.OpenReadStream();


            try
            {
                var imageInfo = Image.Identify(stream);

                if (imageInfo != null)
                {

                    var formatName = imageInfo.Metadata.DecodedImageFormat;
                    response.imageMessage = "Image is valid";
                    response.isSucess = true;

                    loggerTrace.Verbose($"Image {postedFile.FileName} is valid.");
                }
                return response;
            }
            catch (InvalidImageContentException exc)
            {
                loggerTrace.LogError($"Image {postedFile.FileName} validation failed.");
                response.imageMessage = "falied to validate image";
                response.isSucess = false;
                return response;
                throw;
            }

        }
    }
}
