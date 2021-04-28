using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Exceptions;
using Vet.Extensions;
using Vet.Helpers;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class AnimalManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoManager _photoManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnimalManager(IMapper mapper, IUserRepository userRepository, IAnimalRepository animalRepository, ISpeciesRepository speciesRepository, IPhotoManager photoManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _animalRepository = animalRepository;
            _speciesRepository = speciesRepository;
            _mapper = mapper;
            _photoManager = photoManager;
        }

        public async Task<AnimalDto> AddAnimal(AddAnimalDto animal)
        {
            var error = new DataErrorException();
            var ownerId = _httpContextAccessor.GetCurrentUserId();

            ValidationHelper.ValidateEntity(await _userRepository.UserExists(ownerId), "felhasználó");
            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(animal.SpeciesId), "állatfaj");

            ValidationHelper.ValidateData(error, animal.DateOfBirth.ToLocalTime() <= DateTime.Now, "dateOfBirth", "A születési idő nem lehet később, mint az aktuális időpont.");
            ValidationHelper.ValidateData(error, new string[] { "hím", "nőstény" }.Contains(animal.Sex.ToLower()), "sex", "Nincs ilyen nem.");

            var _animal = _mapper.Map<Animal>(animal);

            _animal.OwnerId = ownerId;

            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, ownerId);
            _animal.PhotoPath = photoPath == null ? "Images/Animals/empty-photo.jpg" : photoPath; 
            return _mapper.Map<AnimalDto>(await _animalRepository.AddAnimal(_animal));
        }

        public async Task<AnimalDto> UpdateAnimal(UpdateAnimalDto animal)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(animal.Id), "állat");

            var _animal = await _animalRepository.GetAnimalByIdAsync(animal.Id);

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _speciesRepository.SpeciesExists(animal.SpeciesId), "állatfaj");

            ValidationHelper.ValidateData(error, animal.DateOfBirth.ToLocalTime() <= DateTime.Now, "dateOfBirth", "A születési idő nem lehet később, mint az aktuális időpont.");
            ValidationHelper.ValidateData(error, new string[] { "hím", "nőstény" }.Contains(animal.Sex.ToLower()), "sex", "Nincs ilyen nem.");

            _animal = _mapper.Map<UpdateAnimalDto, Animal>(animal, _animal);
            _animal.DateOfBirth = _animal.DateOfBirth.ToLocalTime();

            return _mapper.Map<AnimalDto>(await _animalRepository.UpdateAnimal(_animal));
        }

        public async Task<AnimalDto> UpdateAnimalPhoto(UpdateAnimalPhotoDto animal)
        {
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(animal.Id), "állat");

            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            var _animal = await _animalRepository.GetAnimalByIdAsync(animal.Id);

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, _animal.OwnerId);
            _animal.PhotoPath = photoPath;
            _animal = await _animalRepository.UpdateAnimal(_animal);
            return _mapper.Map<AnimalDto>(_animal);
        }

        public async Task DeleteAnimalPhoto(int id)
        {
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(id), "állat");

            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            if (!_animal.PhotoPath.Equals("Images/Animals/empty-photo.jpg"))
            {
                _photoManager.RemovePhoto(_animal.PhotoPath);
                _animal.PhotoPath = "Images/Animals/empty-photo.jpg";
            }

            _animal = await _animalRepository.UpdateAnimal(_animal);
        }

        public async Task ChangeStateOfAnimal(int id)
        {
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(id), "állat");

            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            await _animalRepository.ChangeStateOfAnimal(id);
        }

        public async Task<bool> DeleteAnimal(int id)
        {
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(id), "állat"); ;

            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            if (_animal.PhotoPath != "Images/Animals/empty-photo.jpg")
                _photoManager.RemovePhoto(_animal.PhotoPath);

            return await _animalRepository.DeleteAnimal(_animal);
        }

        public async Task<bool> AnimalExists(int id)
            => (await _animalRepository.GetAnimalByIdAsync(id))!=null;

        public async Task<IEnumerable<AnimalDto>> GetAnimalsAsync()
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            return _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetAnimalsAsync());
        }
             

        public async Task<IEnumerable<AnimalDto>> GetArchivedAnimalsByUserIdAsync(string id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1 || loggedInUser.Id == id);

            return _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetArchivedAnimalsByUserId(id));
        }
        public async Task<IEnumerable<AnimalDto>> GetAnimalsByUserIdAsync(string id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1 || loggedInUser.Id == id);

            return _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetAnimalsByUserIdAsync(id));
        }

        public async Task<IEnumerable<AnimalDto>> GetAnimalsByUserEmailAsync(string email)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1 || loggedInUser.Id == (await _userRepository.GetUserIdByUserEmail(email)));
            return _mapper.Map<IEnumerable<AnimalDto>>(await _animalRepository.GetAnimalsByUserEmailAsync(email));
        }

        public async Task<AnimalDto> GetAnimalByIdAsync(int id)
        {
            var error = new DataErrorException();
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(id), "állat");

            var _animal = await _animalRepository.GetAnimalByIdAsync(id);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            return _mapper.Map<AnimalDto>(await _animalRepository.GetAnimalByIdAsync(id));
        }
    }
}
