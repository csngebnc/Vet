using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Interfaces
{
    public interface IAnimalRepository
    {
        Task<bool> AnimalExists(int id);

        //CUD
        Task<Animal> AddAnimal(AddAnimalDto animal, string OwnerId);
        Task<AnimalDto> UpdateAnimal(UpdateAnimalDto animal);
        Task<AnimalDto> UpdateAnimalPhoto(UpdateAnimalPhotoDto animal, string OwnerId);
        Task DeleteAnimalPhoto(int id);
        Task<bool> DeleteAnimal(int id);
        Task ArchiveAnimalById(int id);
        //R
        Task<AnimalDto> GetAnimalByIdAsync(int id);
        Task<IEnumerable<AnimalDto>> GetAnimalsByUserIdAsync(string id);
        Task<IEnumerable<AnimalDto>> GetAnimalsAsync();
        // --> CRUD
    }
}