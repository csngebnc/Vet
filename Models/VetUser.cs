using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Vet.Models
{
    public class VetUser : IdentityUser
    {
        public string RealName { get; set; }
        public string Address { get; set; }
        public string PhotoPath { get; set; }
        public int AuthLevel { get; set; }
        public ICollection<Animal> Animals { get; set; }
        public ICollection<Treatment> Treatments { get; set; }
    }
}
