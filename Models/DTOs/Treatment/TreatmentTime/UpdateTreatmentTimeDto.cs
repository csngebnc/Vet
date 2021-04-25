using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class UpdateTreatmentTimeDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelési idő azonosítójának megadása kötelező.")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés megadása kötelező.")]
        public int TreatmentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezdési időpont (óra) megadása kötelező.")]
        public int StartHour { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezdési időpont (perc) megadása kötelező")]
        public int StartMin { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés végének időpontjának (óra) megadása kötelező.")]
        public int EndHour { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés végének időpontjának (perc) megadása kötelező.")]
        public int EndMin { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés időtartamának megadása kötelező.")]
        public int Duration { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés napjának megadása kötelező.")]
        public int DayOfWeek { get; set; }
    }
}
