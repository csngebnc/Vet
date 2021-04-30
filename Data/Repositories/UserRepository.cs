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
        public UserRepository(VetDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<VetUser> GetUserByIdAsync(string id)
        {
            var user =  await _context.Users.FindAsync(id);
            return user == null ? new VetUser {Id = null, AuthLevel = 0 } : user;
        }

        public async Task<string> GetUserIdByUserEmail(string email)
            => (await _context.Users.SingleOrDefaultAsync(u => u.Email == email))?.Id;

        public async Task<VetUser> GetUserByUsernameAsync(string username)
            => await _context.Users
                .Include(a => a.Animals)
                .SingleOrDefaultAsync(x => x.UserName == username);

        public async Task<IEnumerable<VetUser>> GetUserByFilter(string name, string email)
        {
            IEnumerable<VetUser> user_w_name;
            if(name != null)
            {
                user_w_name = await _context.Users.Where(u => u.RealName.ToLower().Contains(name.ToLower())).ToListAsync();
                if(email != null)
                    user_w_name = user_w_name.Where(u => u.Email.ToLower().Contains(email.ToLower())).ToList();
            }
            else
            {
                if (email != null)
                    user_w_name = await _context.Users.Where(u => u.Email.ToLower().Contains(email.ToLower())).ToListAsync();
                else
                    user_w_name = await _context.Users.ToListAsync();
            }

            return user_w_name;
        }

        public async Task<bool> SetPhoto(string photoPath, string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            user.PhotoPath = photoPath;
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> UserExists(string userId)
            => await _context.Users.AnyAsync(a => a.Id == userId);

    }
}
