using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class Therapia
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<TherapiaRecord> TherapyRecords { get; set; }
    }
}
