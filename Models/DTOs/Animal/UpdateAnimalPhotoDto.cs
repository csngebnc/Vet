using Microsoft.AspNetCore.Http;

namespace Vet.Models.DTOs
{
    public class UpdateAnimalPhotoDto
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
    }
}
