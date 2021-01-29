using Microsoft.AspNetCore.Http;
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
