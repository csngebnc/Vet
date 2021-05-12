using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Pagination;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IUserRepository
    { 
        Task<VetUser> GetUserByIdAsync(string id);
        Task<VetUser> GetUserByUsernameAsync(string username);
        Task<string> GetUserIdByUserEmail(string email);
        IQueryable<VetUser> GetUserByFilter(string name, string email);
        Task<bool> SetPhoto(string photoPath, string userId);

        Task<bool> UserExists(string userId);
    }
}

