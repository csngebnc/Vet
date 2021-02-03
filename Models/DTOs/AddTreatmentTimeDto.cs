using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddTreatmentTimeDto
    {
        public int TreatmentId { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int Duration { get; set; }
        public int DayOfWeek { get; set; }
    }
}
