using System.Collections.Generic;

namespace Vet.Models
{
    public class AnimalSpecies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<Animal> Animals { get; set; }
    }
}
