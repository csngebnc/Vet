using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Interfaces
{
    public interface IPhotoManager
    {
        Task<string> UploadUserPhoto(IFormFile photo, string user_id);
        Task<string> UploadAnimalPhoto(IFormFile photo, string user_id);

        void RemovePhoto(string path);
    }
}
