﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class UpdateTreatmentDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelésazonosító megadása kötelező.")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A kezelés nevének megadása kötelező")]
        public string Name { get; set; }
    }
}
