using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class TreatmentTimeDto
    {
        public int Id { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int Duration { get; set; }
        public int DayOfWeek { get; set; }
        public int TreatmentId { get; set; }
        public bool IsInactive { get; set; }
    }
}
