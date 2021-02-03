using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class TreatmentTime
    {
        public int Id { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int Duration { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsInactive { get; set; }

        [ForeignKey("Treatment")]
        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }
    }
}
