using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class SpeciesManager
    {
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IMapper _mapper;

        public SpeciesManager(IMapper mapper, ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddAnimalSpecies(string name)
        {
            return await _speciesRepository.AddAnimalSpecies(name);
        }

        public async Task<AnimalSpeciesDto> UpdateAnimalSpecies(UpdateAnimalSpeciesDto animal)
        {
            var spec = await _speciesRepository.GetAnimalSpeciesById(animal.Id);
            spec.Name = animal.Name;
            spec = await _speciesRepository.UpdateAnimalSpecies(spec);
            return _mapper.Map<AnimalSpeciesDto>(spec);
        }

        public async Task<bool> DeleteAnimalSpecies(int id)
        {
            var spec = await _speciesRepository.GetAnimalSpeciesByIdWithAnimals(id);
            if (spec.Animals.Count > 0) return false;
            return await _speciesRepository.DeleteAnimalSpecies(spec);
        }

        public async Task ChageStateOfSpecies(int id)
        {
            var spec = await _speciesRepository.GetAnimalSpeciesById(id);
            spec.IsInactive = !spec.IsInactive;
            await _speciesRepository.UpdateAnimalSpecies(spec);
        }

        public async Task<bool> SpeciesExists(string name)
        {
            return (await _speciesRepository.GetAnimalSpeciesByName(name)) != null;
        }

        public async Task<IEnumerable<AnimalSpeciesDto>> GetAnimalSpecies()
            => _mapper.Map<IEnumerable<AnimalSpeciesDto>>(await _speciesRepository.GetAnimalSpecies());

        public async Task<AnimalSpeciesDto> GetAnimalSpeciesById(int id)
            => _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.GetAnimalSpeciesById(id));

        public async Task<AnimalSpeciesDto> GetAnimalSpeciesByName(string name)
            => _mapper.Map<AnimalSpeciesDto>(await _speciesRepository.GetAnimalSpeciesByName(name));
    }
}
