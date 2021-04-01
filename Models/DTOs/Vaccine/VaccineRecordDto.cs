using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class VaccineRecordDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int VaccineId { get; set; }
        public string VaccineName { get; set; }

        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
    }
}
