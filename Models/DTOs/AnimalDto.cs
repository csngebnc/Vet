using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalDto : ControllerBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string Sex { get; set; }
        public int SpeciesId { get; set; }
        public string SpeciesName { get; set; }
        public string SubSpecies { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string PhotoPath { get; set; }
    }
}
