using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly VetDbContext _context;

        public SpeciesRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<AnimalSpecies> AddAnimalSpecies(string name)
        {
            var spec = new AnimalSpecies { Name = name };
            _context.AnimalSpecies.Add(spec);
            if ((await _context.SaveChangesAsync()) > 0)
                return spec;
            return null;

        }
        public async Task<AnimalSpecies> UpdateAnimalSpecies(AnimalSpecies spec)
        {
            _context.AnimalSpecies.Update(spec);
            await _context.SaveChangesAsync();
            return await this.GetAnimalSpeciesById(spec.Id);
        }
        public async Task<bool> DeleteAnimalSpecies(AnimalSpecies spec)
        {
            _context.AnimalSpecies.Remove(spec);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<IEnumerable<AnimalSpecies>> GetAnimalSpecies()
            => await _context.AnimalSpecies.ToListAsync();

        public async Task<AnimalSpecies> GetAnimalSpeciesById(int id)
            => await _context.AnimalSpecies.FindAsync(id);

        public async Task<AnimalSpecies> GetAnimalSpeciesByIdWithAnimals(int id)
            => await _context.AnimalSpecies.Include(a => a.Animals).Where(s => s.Id == id).FirstOrDefaultAsync();

        public async Task<AnimalSpecies> GetAnimalSpeciesByName(string name)
            => await _context.AnimalSpecies
                .Where(s => s.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

        public async Task<bool> SpeciesExists(int specId)
            => await _context.AnimalSpecies.AnyAsync(a => a.Id == specId);

        public async Task<bool> SpeciesExistsByName(string name)
            => await _context.AnimalSpecies.AnyAsync(a => a.Name.Trim().ToLower() == name.Trim().ToLower());
    }
}
