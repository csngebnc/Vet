using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class AnimalSpecies
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Animal> Animals { get; set; }
    }
}
