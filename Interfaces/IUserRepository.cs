using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IUserRepository
    { 
        Task<VetUser> GetUserByIdAsync(string id);
        Task<VetUser> GetUserByUsernameAsync(string username);
    }
}

