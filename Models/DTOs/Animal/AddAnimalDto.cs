using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vet.Models.DTOs
{
    public class AddAnimalDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat nevének megadása kötelező.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A születési dátum megadása kötelező.")]
        public DateTime DateOfBirth { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat nemének megadása kötelező.")]
        public string Sex { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat fajának megadása kötelező.")]
        public int SpeciesId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
