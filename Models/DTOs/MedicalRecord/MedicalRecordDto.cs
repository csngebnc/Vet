using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string DoctorId { get; set; }
        public string DoctorName { get; set; }

        public string OwnerEmail { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }

        public int? AnimalId { get; set; }
        public string AnimalName { get; set; }

        public string Anamnesis { get; set; }
        public string Symptoma { get; set; }
        public string Details { get; set; }
        public ICollection<MedicalRecordPhotoDto> Photos { get; set; }
        public ICollection<TherapiaRecordDto> TherapiaRecords { get; set; }

    }
}
