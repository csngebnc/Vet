using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class UpdateVaccineRecordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Oltásazonosító megadása kötelező")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Állat megadása kötelező")]
        public int AnimalId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dátum megadása kötelező")]
        public DateTime Date { get; set; }

    }
}
