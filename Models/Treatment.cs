using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public VetUser Doctor { get; set; }

        public ICollection<TreatmentTime> TreatmentTimes { get; set; }
    }
}
