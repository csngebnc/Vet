﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddAppointmentByUserDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TreatmentId { get; set; }

        public string DoctorId { get; set; }

        public int? AnimalId { get; set; }
    }
}