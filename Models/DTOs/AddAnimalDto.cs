using Microsoft.AspNetCore.Http;
using System;

namespace Vet.Models.DTOs
{
    public class AddAnimalDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public int SpeciesId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
