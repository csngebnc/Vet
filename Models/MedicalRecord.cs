using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string DoctorId { get; set; }
        public VetUser Doctor { get; set; }

        public string OwnerEmail { get; set; }
        public string OwnerId { get; set; }
        public VetUser Owner { get; set; }

        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }

        public string Anamnesis { get; set; }
        public string Symptoma { get; set; }
        public string Details { get; set; }

        public ICollection<MedicalRecordPhoto> Photos { get; set; }
        public ICollection<TherapiaRecord> TherapiaRecords { get; set; }
    }
}
