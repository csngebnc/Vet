using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddAppointmentDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az időpont kezdetének megadása kötelező.")]
        public DateTime StartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az időpont végének megadása kötelező.")]
        public DateTime EndDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés kiválasztása kötelező.")]
        public int TreatmentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doktor kiválasztása kötelező.")]
        public string DoctorId { get; set; }
        public string? OwnerId { get; set; }
        public int? AnimalId { get; set; }
    }
}
