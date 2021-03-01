using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public string DoctorId { get; set; }
        public VetUser Doctor { get; set; }

        public string OwnerId { get; set; }
        public VetUser Owner { get; set; }

        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }
        public string Details { get; set; }

        public bool IsResigned { get; set; }
    }
}
