using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;
using Vet.BL.Exceptions;
using Vet.Helpers;
using Vet.Extensions;

namespace Vet.BL
{
    public class SpeciesManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IMapper _mapper;

        public SpeciesManager(IMapper mapper, IUserRepository userRepository, ISpeciesRepository speciesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _speciesRepository = speciesRepository;
            _mapper = mapper;
        }

        public async Task<AnimalSpeciesDto> AddAnimalSpecies(string name)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, !(await _speciesRepository.SpeciesExistsByName(name)), "speciesId", "A megadott néven már rögzítésre került állatfaj.");
            return _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.AddAnimalSpecies(name));
        }
        public async Task<AnimalSpeciesDto> UpdateAnimalSpecies(UpdateAnimalSpeciesDto animal)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _speciesRepository.SpeciesExists(animal.Id), "speciesId", "A megadott azonosítóval nem létezik állatfaj.");
            ValidationHelper.ValidateData(error, !(await _speciesRepository.SpeciesExistsByName(animal.Name)), "speciesId", "A megadott néven már rögzítésre került állatfaj.");
            var spec = await _speciesRepository.GetAnimalSpeciesById(animal.Id);
            spec.Name = animal.Name;
            spec = await _speciesRepository.UpdateAnimalSpecies(spec);
            return _mapper.Map<AnimalSpeciesDto>(spec);
        }

        public async Task<bool> DeleteAnimalSpecies(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            ValidationHelper.ValidateData(error, await _speciesRepository.GetAnimalSpeciesById(id) != null, "speciesId", "A megadott azonosítóval nem létezik állatfaj.");
            var spec = await _speciesRepository.GetAnimalSpeciesByIdWithAnimals(id);
            if (spec.Animals.Count > 0) return false;
            return await _speciesRepository.DeleteAnimalSpecies(spec);
        }

        public async Task ChageStateOfSpecies(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _speciesRepository.SpeciesExists(id), "speciesId", "A megadott azonosítóval nem létezik állatfaj.");
            var spec = await _speciesRepository.GetAnimalSpeciesById(id);
            spec.IsInactive = !spec.IsInactive;
            await _speciesRepository.UpdateAnimalSpecies(spec);
        }

        public async Task<IEnumerable<AnimalSpeciesDto>> GetAnimalSpecies()
            => _mapper.Map<IEnumerable<AnimalSpeciesDto>>(await _speciesRepository.GetAnimalSpecies());

        public async Task<AnimalSpeciesDto> GetAnimalSpeciesById(int id)
            => _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.GetAnimalSpeciesById(id));

    }
}
