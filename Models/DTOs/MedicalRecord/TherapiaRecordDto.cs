using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class TherapiaRecordDto
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public int TherapiaId { get; set; }
        public string TherapiaName { get; set; }
        public string TherapiaUnit { get; set; }

        public double Amount { get; set; }
    }
}
