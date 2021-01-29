using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class AnimalManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoManager _photoManager;

        public AnimalManager(IMapper mapper, IUserRepository userRepository, IAnimalRepository animalRepository, IPhotoManager photoManager)
        {
            _userRepository = userRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
            _photoManager = photoManager;
        }

        public async Task<Animal> AddAnimal(AddAnimalDto animal, string OwnerId)
        {
            var _animal = _mapper.Map<Animal>(animal);
            _animal.OwnerId = OwnerId;

            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, OwnerId);
            _animal.PhotoPath = photoPath == null ? "Images/Animals/empty-photo.jpg" : photoPath;
            return await _animalRepository.AddAnimal(_animal);
        }

        public async Task<AnimalDto> UpdateAnimal(UpdateAnimalDto animal)
        {
            var _animal = await _animalRepository.GetAnimalByIdAsync(animal.Id);
            _animal = _mapper.Map<UpdateAnimalDto, Animal>(animal, _animal);
            _animal = await _animalRepository.UpdateAnimal(_animal);

            return _mapper.Map<AnimalDto>(_animal);
        }

        public async Task<AnimalDto> UpdateAnimalPhoto(UpdateAnimalPhotoDto animal, string OwnerId)
        {
            var _animal = await _animalRepository.GetAnimalByIdAsync(animal.Id);
            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, OwnerId);
            _animal.PhotoPath = photoPath;

            _animal = await _animalRepository.UpdateAnimal(_animal);
            return _mapper.Map<AnimalDto>(_animal);
        }

        public async Task DeleteAnimalPhoto(int id)
        {
            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (!_animal.PhotoPath.Equals("Images/Animals/empty-photo.jpg"))
            {
                _photoManager.RemovePhoto(_animal.PhotoPath);
                _animal.PhotoPath = "Images/Animals/empty-photo.jpg";
            }

            _animal = await _animalRepository.UpdateAnimal(_animal);
        }

        public Task ArchiveAnimalById(int id) { /* később */ throw new NotImplementedException(); }

        public async Task<bool> DeleteAnimal(int id)
        {
            // TODO: bővíteni
            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (_animal.PhotoPath != "Images/Animals/empty-photo.jpg")
                _photoManager.RemovePhoto(_animal.PhotoPath);

            return await _animalRepository.DeleteAnimal(_animal);
        }

        public async Task<bool> AddAnimalSpecies(string name)
        {
            return await _animalRepository.AddAnimalSpecies(name);
        }

        public async Task<AnimalSpeciesDto> UpdateAnimalSpecies(AnimalSpeciesDto animal)
        {
            var spec = await _animalRepository.GetAnimalSpeciesById(animal.Id);
            spec.Name = animal.Name;
            spec = await _animalRepository.UpdateAnimalSpecies(spec);
            return _mapper.Map<AnimalSpeciesDto>(spec);
        }

        public async Task<bool> DeleteAnimalSpecies(int id)
        {
            var spec = await _animalRepository.GetAnimalSpeciesById(id);
            return await _animalRepository.DeleteAnimalSpecies(spec);
        }

        public async Task<bool> AnimalExists(int id)
        {
            return (await _animalRepository.GetAnimalByIdAsync(id))!=null;
        }

        public async Task<bool> SpeciesExists(string name)
        {
            return (await _animalRepository.GetAnimalSpeciesByName(name)) != null;
        }


        public async Task<IEnumerable<AnimalDto>> GetAnimalsAsync()
            => _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetAnimalsAsync());

        public async Task<IEnumerable<AnimalDto>> GetAnimalsByUserIdAsync(string id)
            => _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetAnimalsByUserIdAsync(id));

        public async Task<AnimalDto> GetAnimalByIdAsync(int id)
            => _mapper.Map<AnimalDto>(await _animalRepository.GetAnimalByIdAsync(id));
    }
}
