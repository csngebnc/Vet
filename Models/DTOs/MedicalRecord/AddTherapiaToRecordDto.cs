using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class AddTherapiaToRecordDto
    {
        public int MedicalRecordId { get; set; }
        public int TherapiaId { get; set; }
        public double Amount { get; set; }
    }
}
