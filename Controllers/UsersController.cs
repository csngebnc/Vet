using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.BL;
using Vet.BL.Exceptions;
using Vet.BL.Pagination;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly UsersManager _userManager;

        public UsersController(UsersManager userManager, IPhotoManager photoManager, IMapper mapper)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<VetUserDto>> GetCurrentUser()
        {
            return Ok(await _userManager.GetUserById(User.GetById()));
        }

        [HttpGet("filter")]
        public async Task<PagedList<VetUserDto>> GetUsersByFilter([FromQuery]string name, [FromQuery] string email, [FromQuery]PaginationData pgd)
        {
            var a = name;
            return await _userManager.GetUsersByFilter(name, email, pgd);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VetUserDto>> GetUserById(string id)
        {
            return await _userManager.GetUserById(id);
        }

        [HttpGet("get-id/{email}")]
        public async Task<VetUserDto> GetUserIdByUserEmail(string email)
        {
            return await _userManager.GetUserByEmail(email);
        }

        [HttpGet("role")]
        public async Task<int> GetRole()
        {
            return await _userManager.GetCurrentAuthLevel(User.GetById());
        }
        

        [HttpPut("add-photo")]
        public async Task<ActionResult<string>> UploadPhoto([FromForm]PhotoClass profilePhoto)
        {
            return await _userManager.AddUserPhoto(profilePhoto, User.GetById());
        }

        [HttpPut("delete-photo")]
        public async Task<bool> DeletePhoto()
        {
            return await _userManager.DeleteUserPhoto(User.GetById());
        }

    }
}
