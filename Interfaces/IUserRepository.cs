using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IUserRepository
    { 
        Task<VetUser> GetUserByIdAsync(string id);
        Task<VetUser> GetUserByUsernameAsync(string username);
        Task<string> GetUserIdByUserEmail(string email);
        Task<IEnumerable<VetUser>> GetUserByFilter(string name, string email);
    }
}

