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
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntityAlreadyExists(!(await _speciesRepository.SpeciesExistsByName(name)), "A megadott néven már rögzítésre került állatfaj.");
            return _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.AddAnimalSpecies(name));
        }
        public async Task<AnimalSpeciesDto> UpdateAnimalSpecies(UpdateAnimalSpeciesDto animal)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(animal.Id), "állatfaj");
            ValidationHelper.ValidateEntityAlreadyExists(!(await _speciesRepository.SpeciesExistsByName(animal.Name)), "A megadott néven már rögzítésre került állatfaj.");

            var spec = await _speciesRepository.GetAnimalSpeciesById(animal.Id);
            spec.Name = animal.Name;
            spec = await _speciesRepository.UpdateAnimalSpecies(spec);
            return _mapper.Map<AnimalSpeciesDto>(spec);
        }

        public async Task<bool> DeleteAnimalSpecies(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(id), "állatfaj");
            var spec = await _speciesRepository.GetAnimalSpeciesByIdWithAnimals(id);
            ValidationHelper.ValidateEntityAlreadyExists(spec.Animals.Count > 0, "A megadott állatfajhoz már tartoznak állatok.");

            return await _speciesRepository.DeleteAnimalSpecies(spec);
        }

        public async Task ChageStateOfSpecies(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(id), "állatfaj");

            var spec = await _speciesRepository.GetAnimalSpeciesById(id);
            spec.IsInactive = !spec.IsInactive;
            await _speciesRepository.UpdateAnimalSpecies(spec);
        }

        public async Task<IEnumerable<AnimalSpeciesDto>> GetAnimalSpecies()
            => _mapper.Map<IEnumerable<AnimalSpeciesDto>>(await _speciesRepository.GetAnimalSpecies());

        public async Task<AnimalSpeciesDto> GetAnimalSpeciesById(int id)
        {
            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(id), "állatfaj");
            return  _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.GetAnimalSpeciesById(id));
        }

    }
}
