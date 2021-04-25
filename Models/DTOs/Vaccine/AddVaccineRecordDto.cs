using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class AddVaccineRecordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dátum megadása kötelező.")]
        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vakcina megadása kötelező")]
        public int VaccineId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Állat megadása")]
        public int AnimalId { get; set; }
    }
}
