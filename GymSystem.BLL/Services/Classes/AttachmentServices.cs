using GymSystem.BLL.Services.Intrterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AttachmentServices : IAttachmentServices
    {
        public AttachmentServices(ILogger<AttachmentServices> logger , IWebHostEnvironment env )
        {
            this.logger = logger;
            this.env = env;
        }
        private readonly long maxFileSize = 5 * 1024 * 1024;
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachmentServices> logger;
        private readonly IWebHostEnvironment env;

        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName) ) 
            {
                return false;
            }
            try
            {
                var fullPath = Path.Combine(env.ContentRootPath ,folderName , fileName);
                if (!File.Exists(fullPath)) { return false;  }
                File.Delete(fullPath);
                return true;
            }
            catch(Exception ex)    
            {
                logger.LogError("Failed to Delete the Attachment"); 
                return false;
            }
        }

        public (Stream stream, string ContentType) GetFile(string fileName, string folderName)
        {
            var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File '{fileName}' not found.");
            }

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fullPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName)
        {
            if(fileStream == null || !fileStream.CanRead)
            {
                return null;
            }
            if(fileStream.Length == 0)
            {
                return null ;
            }
            if (fileStream.Length > maxFileSize) 
            {
                logger.LogWarning("Rejected File Too Large");
                return null;
            }
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                logger.LogWarning("Rejecte wrong Extension File");
                return null;
            }

            var UploadedFolder = Path.Combine(env.ContentRootPath , folderName) ;
            Directory.CreateDirectory(UploadedFolder);
            var StoredFileName = $"{Guid.NewGuid()}{extension}"; 
            var FilePath = Path.Combine(UploadedFolder, StoredFileName); // Full Path 
            try
            {
                await using var fs = new FileStream(FilePath,FileMode.CreateNew, FileAccess.Write , FileShare.None); 
                await fileStream.CopyToAsync(fs);
                return StoredFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " failed to upload file");
                return null;
            }
        }
    }
}
