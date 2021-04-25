using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Exceptions;
using Vet.Helpers;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class UsersManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoManager _photoManager;

        public UsersManager(IMapper mapper, IUserRepository userRepository, IAnimalRepository animalRepository, ISpeciesRepository speciesRepository, IPhotoManager photoManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoManager = photoManager;
        }

        public async Task<VetUserDto> GetUserById(string userId)
        {
            return _mapper.Map<VetUserDto>(await _userRepository.GetUserByIdAsync(userId));
            
        }

        public async Task<VetUserDto> GetUserByEmail(string email)
        {
            var id = await _userRepository.GetUserIdByUserEmail(email);
            if (id == null) throw new EntityNotFoundException();
            return _mapper.Map<VetUserDto>(await _userRepository.GetUserByIdAsync(id));
        }

        public async Task<int> GetCurrentAuthLevel(string currentId)
        {
            var user = await _userRepository.GetUserByIdAsync(currentId);
            return user == null ? 0 : user.AuthLevel;
        }

        public async Task<IEnumerable<VetUserDto>> GetUsersByFilter(string email, string name)
        {
            return _mapper.Map<IEnumerable<VetUserDto>>(await _userRepository.GetUserByFilter(name, email));
        }

        public async Task<string> AddUserPhoto(PhotoClass picData, string userId)
        {
            var path = await _photoManager.UploadUserPhoto(picData.ProfilePhoto, userId);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (path == null)
                throw new DataErrorException("Sikertelen feltöltés");

            if (user.PhotoPath != null)
                _photoManager.RemovePhoto(user.PhotoPath);

            if (await _userRepository.SetPhoto(path, userId))
                return path;
            else
                throw new DataErrorException("Sikertelen feltöltés");
        }

        public async Task<bool> DeleteUserPhoto(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            await _userRepository.SetPhoto(null, user.Id);
            return _photoManager.RemovePhoto(user.PhotoPath);
        }

    }

    
}
