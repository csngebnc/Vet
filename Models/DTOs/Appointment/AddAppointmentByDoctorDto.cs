using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Appointment
{
    public class AddAppointmentByDoctorDto
    {
        public string OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TreatmentId { get; set; }

        public string DoctorId { get; set; }

        public int? AnimalId { get; set; }
    }
}
