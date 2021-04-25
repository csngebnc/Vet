using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class UpdateTherapiaOnRecordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés azonosítójának megadása kötelező.")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Kórlap azonosítójának megadása kötelező.")]
        public int MedicalRecordId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Kezelés azonosítójának megadása kötelező.")]
        public int TherapiaId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mennyiség megadása kötelező.")]
        public double Amount { get; set; }
    }
}
