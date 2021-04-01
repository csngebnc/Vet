using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models.DTOs.MedicalRecord;

namespace Vet.Models.DTOs
{
    public class AddMedicalRecordDto
    {
        public DateTime Date { get; set; }

        public string OwnerEmail { get; set; }

        public int? AnimalId { get; set; }

        public string Anamnesis { get; set; }
        public string Symptoma { get; set; }
        public string Details { get; set; }

        public TherapiaOnMedicalRecord[] Therapias { get; set; }

    }
}
