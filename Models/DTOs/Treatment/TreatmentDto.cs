using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class TreatmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
    }
}
