using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddHolidayDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A szabadság első napjának megadása kötelező.")]
        public DateTime StartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A szabadság utolsó aktív napjának megadása kötelező.")]
        public DateTime EndDate { get; set; }
    }
}
