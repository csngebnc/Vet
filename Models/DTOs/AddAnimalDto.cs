using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
