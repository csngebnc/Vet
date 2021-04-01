using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class Vaccine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        ICollection<VaccineRecord> Records { get; set; }

    }
}
