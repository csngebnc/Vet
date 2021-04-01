using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class VaccineRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int VaccineId { get; set; }
        public Vaccine Vaccine { get; set; }

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
    }
}
