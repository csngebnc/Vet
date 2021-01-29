using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IAnimalRepository
    {
        //CUD
        Task<Animal> AddAnimal(Animal animal);
        Task<Animal> UpdateAnimal(Animal animal);
        Task<bool> DeleteAnimal(Animal animal);
        Task ArchiveAnimalById(int id);
        //R
        Task<Animal> GetAnimalByIdAsync(int id);
        Task<IEnumerable<Animal>> GetAnimalsByUserIdAsync(string id);
        Task<IEnumerable<Animal>> GetAnimalsAsync();
        // --> CRUD


        Task<bool> AddAnimalSpecies(string name);
        Task<AnimalSpecies> UpdateAnimalSpecies(AnimalSpecies spec);
        Task<bool> DeleteAnimalSpecies(AnimalSpecies spec);

        Task<IEnumerable<AnimalSpecies>> GetAnimalSpecies();
        Task<AnimalSpecies> GetAnimalSpeciesById(int id);
        Task<AnimalSpecies> GetAnimalSpeciesByName(string name);
    }
}