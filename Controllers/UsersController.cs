using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    }
}
