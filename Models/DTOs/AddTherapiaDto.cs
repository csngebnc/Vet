using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class AddTherapiaDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés nevének megadása kötelező.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelési egység nevének megadása kötelező.")]
        public string UnitName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés egységének megadása kötelező.")]
        public double Unit { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az egységár megadása kötelező.")]
        public double PricePerUnit { get; set; }
        public bool IsInactive { get; set; }
    }
}
