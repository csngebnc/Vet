using System;
using System.ComponentModel.DataAnnotations;

namespace Vet.Models.DTOs
{

    public class UpdateAnimalDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat azonosítójának megadása kötelező.")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat nevének megadása kötelező.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A születési dátum megadása kötelező.")]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat nemének megadása kötelező.")]
        public string Sex { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az állat fajának megadása kötelező.")]
        public int SpeciesId { get; set; }

        public double Weight { get; set; }
        public string SubSpecies { get; set; }
    }
}
