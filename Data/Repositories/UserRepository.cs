using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VetDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(VetDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VetUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<VetUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(a => a.Animals)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
    }
}
