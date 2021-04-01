using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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
        public async Task<string> GetUserIdByUserEmail(string email)
        {
            return await _userRepository.GetUserIdByUserEmail(email);
        }

        [HttpGet("role")]
        public async Task<int> GetRole()
        {
            var user = (await _userRepository.GetUserByIdAsync(User.GetById()));
            return user == null ? 0 : user.AuthLevel;
        }

        [HttpGet("fix-error")]
        public async Task<ActionResult<string>> Feladat()
        {
            ModelState.AddModelError("hiba", "hiba");
            ModelState.AddModelError("hibababa", "hibababa");
            return BadRequest(ModelState);
        }

    }
}
