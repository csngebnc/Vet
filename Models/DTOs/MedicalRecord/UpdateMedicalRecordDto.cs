using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class UpdateMedicalRecordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kórlap azonosítójának megadása kötelező.")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kórlap létrehozásának dátuma kötelező.")]
        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A gazdi e-mail címének megadása kötelező.")]
        public string OwnerEmail { get; set; }

        public int? AnimalId { get; set; }

        public string Anamnesis { get; set; }
        public string Symptoma { get; set; }
        public string Details { get; set; }

        public TherapiaOnMedicalRecord[] Therapias { get; set; }
    }
}
