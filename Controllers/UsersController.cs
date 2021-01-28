using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vet.Data;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models;
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
