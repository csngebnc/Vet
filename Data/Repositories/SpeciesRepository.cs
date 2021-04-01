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

        public async Task<bool> AddAnimalSpecies(string name)
        {
            _context.AnimalSpecies.Add(new AnimalSpecies { Name = name });
            return (await _context.SaveChangesAsync()) > 0;

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
        {
            return await _context.AnimalSpecies.ToListAsync();
        }

        public async Task<AnimalSpecies> GetAnimalSpeciesById(int id)
        {
            return await _context.AnimalSpecies.FindAsync(id);
        }

        public async Task<AnimalSpecies> GetAnimalSpeciesByIdWithAnimals(int id)
        {
            return await _context.AnimalSpecies.Include(a => a.Animals).Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AnimalSpecies> GetAnimalSpeciesByName(string name)
        {
            return await _context.AnimalSpecies
                .Where(s => s.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
