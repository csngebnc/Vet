using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Vet.Models.DTOs
{
    public class UpdateAnimalPhotoDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat azonosítójának megadása kötelező.")]
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
    }
}
