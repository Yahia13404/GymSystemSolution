using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Intrterfaces
{
    public interface IAttachmentServices
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName);
        bool Delete(string fileName , string folderName);
        (Stream stream , string ContentType) GetFile(string fileName , string folderName);
    }
}
