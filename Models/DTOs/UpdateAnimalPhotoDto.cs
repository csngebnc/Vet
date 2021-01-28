using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs
{
    public class UpdateAnimalPhotoDto
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
    }
}
