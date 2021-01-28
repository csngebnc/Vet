using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;

namespace Vet.Helpers
{
    public class PhotoManager : IPhotoManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PhotoManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadAnimalPhoto(IFormFile photo, string user_id)
        {
            string path = null;

            if (photo != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Animals");
                string newFileName = user_id + "-" + Guid.NewGuid().ToString("N") + ".png";

                path = Path.Combine("Images", "Animals", newFileName);

                using (var fileStream = new FileStream(Path.Combine(folder, newFileName), FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
            }
            return path;
        }

        public async Task<string> UploadUserPhoto(IFormFile photo, string user_id)
        {
            string path = null;

            if (photo != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Users");
                string newFileName = user_id + Path.GetExtension(photo.FileName);

                path = Path.Combine("Images", "Users", newFileName);

                using (var fileStream = new FileStream(Path.Combine(folder, newFileName), FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
            }
            return path;
        }

        public void RemovePhoto(string path)
        {
            if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, path)))
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, path));

        }
    }
}
