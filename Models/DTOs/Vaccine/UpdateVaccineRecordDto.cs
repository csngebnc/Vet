using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.Vaccine
{
    public class UpdateVaccineRecordDto
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public DateTime Date { get; set; }

    }
}
