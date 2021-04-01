using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class TherapiaRecord
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public int TherapiaId { get; set; }
        public Therapia Therapia { get; set; }

        public double Amount { get; set; }
    }
}
