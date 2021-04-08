using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoManager _photoManager;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IPhotoManager photoManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoManager = photoManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<VetUserDto>> GetCurrentUser()
        {
            return _mapper.Map<VetUserDto>(await _userRepository.GetUserByIdAsync(User.GetById()));
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<VetUserDto>> GetUsersByFilter([FromQuery]string name, [FromQuery] string email)
        {
            return _mapper.Map<IEnumerable<VetUserDto>>(await _userRepository.GetUserByFilter(name, email));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VetUserDto>> GetUserById(string id)
        {
            return _mapper.Map<VetUserDto>(await _userRepository.GetUserByIdAsync(id));
        }

        [HttpGet("get-id/{email}")]
        public async Task<VetUserDto> GetUserIdByUserEmail(string email)
        {
            var id = await _userRepository.GetUserIdByUserEmail(email);
            if (id == null) return null;
            return _mapper.Map<VetUserDto>(await _userRepository.GetUserByIdAsync(id));
        }

        [HttpGet("role")]
        public async Task<int> GetRole()
        {
            var user = (await _userRepository.GetUserByIdAsync(User.GetById()));
            return user == null ? 0 : user.AuthLevel;
        }
        /*
        [HttpGet("fix-error")]
        public async Task<ActionResult<string>> Feladat()
        {
            ModelState.AddModelError("hiba", "hiba");
            ModelState.AddModelError("hibababa", "hibababa");
            return BadRequest(ModelState);
        }
        */
        [HttpPut("add-photo")]
        public async Task<ActionResult<string>> UploadPhoto([FromForm]TestClass profilePhoto)
        {
            var path = await _photoManager.UploadUserPhoto(profilePhoto.ProfilePhoto, User.GetById());

            var user = await _userRepository.GetUserByIdAsync(User.GetById());

            if (path == null) return BadRequest("Sikertelen képfeltöltés!!!");

            if(user.PhotoPath != null)
                _photoManager.RemovePhoto(user.PhotoPath);

            if (await _userRepository.SetPhoto(path, User.GetById()))
            {
                return path;
            }
            else
            {
                return BadRequest("Sikertelen képfeltöltés");
            }
        }

        [HttpPut("delete-photo")]
        public async Task<bool> DeletePhoto()
        {
            var user = await _userRepository.GetUserByIdAsync(User.GetById());
            await _userRepository.SetPhoto(null, user.Id);
            return _photoManager.RemovePhoto(user.PhotoPath);
        }

    }
}
