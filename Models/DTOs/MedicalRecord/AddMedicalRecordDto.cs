using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models.DTOs.MedicalRecord;

namespace Vet.Models.DTOs
{
    public class AddMedicalRecordDto
    {
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
