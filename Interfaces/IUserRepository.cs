using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Interfaces
{
    public interface IUserRepository
    { 
        Task<VetUser> GetUserByIdAsync(string id);
        Task<VetUser> GetUserByUsernameAsync(string username);
    }
}

