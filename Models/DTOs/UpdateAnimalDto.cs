using System;

namespace Vet.Models.DTOs
{

    public class UpdateAnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Weight { get; set; }
        public string Sex { get; set; }
        public int SpeciesId { get; set; }
        public string SubSpecies { get; set; }
    }
}
