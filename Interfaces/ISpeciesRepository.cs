﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface ISpeciesRepository
    {
        Task<AnimalSpecies> AddAnimalSpecies(string name);
        Task<AnimalSpecies> UpdateAnimalSpecies(AnimalSpecies spec);
        Task<bool> DeleteAnimalSpecies(AnimalSpecies spec);

        Task<IEnumerable<AnimalSpecies>> GetAnimalSpecies();
        Task<AnimalSpecies> GetAnimalSpeciesById(int id);
        Task<AnimalSpecies> GetAnimalSpeciesByIdWithAnimals(int id);
        Task<AnimalSpecies> GetAnimalSpeciesByName(string name);
        Task<bool> SpeciesExists(int specId);
        Task<bool> SpeciesExistsByName(string name);
    }
}
