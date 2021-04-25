using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class AddVaccineDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az oltás nevének megadása kötelező.")]
        public string Name { get; set; }
    }
}
