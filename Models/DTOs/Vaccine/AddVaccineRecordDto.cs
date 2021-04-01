using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class AddVaccineRecordDto
    {
        public DateTime Date { get; set; }

        public int VaccineId { get; set; }

        public int AnimalId { get; set; }
    }
}
