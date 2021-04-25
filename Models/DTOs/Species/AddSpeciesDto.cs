using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddSpeciesDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A faj nevének megadása kötelező.")]
        public string Name { get; set; }
    }
}
