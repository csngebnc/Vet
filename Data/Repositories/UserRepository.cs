using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var user =  await _context.Users.FindAsync(id);
            return user == null ? new VetUser { AuthLevel = 0 } : user;
        }

        public async Task<string> GetUserIdByUserEmail(string email)
        {
            return (await _context.Users.SingleOrDefaultAsync(u => u.Email == email))?.Id;
        }

        public async Task<VetUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(a => a.Animals)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<VetUser>> GetUserByFilter(string name, string email)
        {
            if (name == null) name = "";
            if (email == null) email = "";
            
            return await _context.Users.Where(u => u.RealName.Contains(name) && u.Email.Contains(email)).ToListAsync();
        }

        public async Task<bool> SetPhoto(string photoPath, string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            user.PhotoPath = photoPath;
            return (await _context.SaveChangesAsync() > 0);
        }

    }
}
