using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class TherapiaRepository : ITherapiaRepository
    {
        private readonly VetDbContext _context;
        public TherapiaRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<Therapia> AddTherapia(Therapia therapia)
        {
            _context.Therapias.Add(therapia);
            if ((await _context.SaveChangesAsync()) > 0)
                return therapia;
            return null;
        }

        public async Task<Therapia> UpdateTherapia(Therapia therapia)
        {
            _context.Therapias.Update(therapia);
            await _context.SaveChangesAsync();
            return await this.GetTherapiaById(therapia.Id);
        }

        public async Task<bool> DeleteTherapia(Therapia therapia)
        {
            _context.Therapias.Remove(therapia);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<Therapia> GetTherapiaById(int id)
            => await _context.Therapias.Include(t => t.TherapyRecords).Where(t => t.Id == id).SingleOrDefaultAsync();

        public async Task<IEnumerable<Therapia>> GetTherapias()
            => await _context.Therapias.Include(t => t.TherapyRecords).ToListAsync();
        public async Task<bool> TherapiaExists(int photoId)
            => await _context.Therapias.AnyAsync(a => a.Id == photoId);
    }

}
