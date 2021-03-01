using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string TreatmentName { get; set; }

        public string DoctorName { get; set; }

        public string OwnerId { get; set; }
        public string OwnerName { get; set; }

        public int? AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string Details { get; set; }
        public bool IsResigned { get; set; }
    }
}
