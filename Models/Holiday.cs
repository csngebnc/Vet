using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public VetUser Doctor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
